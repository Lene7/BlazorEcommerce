
using BlazorEccomerce.Server.Migrations;
using System.Text.Json;

namespace BlazorEccomerce.Server.Services.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly DataContext _context;
		private readonly ICartService _cartService;

		public OrderService(DataContext context, ICartService cartService)
        {
            _context = context;
			_cartService = cartService;
        }

        public async Task<ServiceResponse<OrderDetailsResponseDTO>> GetOrderDetails(int userId, int orderId)
        {
            var response = new ServiceResponse<OrderDetailsResponseDTO>
            {
                Data = new OrderDetailsResponseDTO
                {
                    Products = new List<OrderDetailsProductResponseDTO>()
                }
            };

            var order = await _context.Orders
                .Where(o => o.Id == orderId && o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.ProductType)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                response.Message = "Order not found.";
                response.Success = false;
                return response;
            }

            response.Data.OrderDate = order.OrderDate; 
            response.Data.TotalPrice = order.TotalPrice;

            foreach (var orderItem in order.OrderItems)
            {
                var productVariant = orderItem.ProductVariant;

                if (productVariant == null || productVariant.Product == null || productVariant.ProductType == null)
                {
                    continue;
                }

                var orderProduct = new OrderDetailsProductResponseDTO
                {
                    ProductVariantId = productVariant.ProductVariantId,
                    ProductTitle = productVariant.Product.Title,
                    ProductTypeName = productVariant.ProductType.Name,
                    ProductImageURL = productVariant.Product.ImageUrl,
                    Quantity = orderItem.Quantity,
                    TotalPrice = orderItem.TotalPrice
                };

                response.Data.Products.Add(orderProduct);
            }

            response.Success = response.Data.Products.Any();
            if (!response.Success)
            {
                response.Message = "No products found.";
            }

            return response;
        }

			public async Task<ServiceResponse<List<OrderOverviewResponseDTO>>> GetOrders(int userId)
        {
            var response = new ServiceResponse<List<OrderOverviewResponseDTO>>();

            try
            {
                var orders = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                    .ThenInclude(pv  => pv.Product)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                if (orders == null || !orders.Any())
                {
                    response.Success = false;
                    response.Message = "No orders found.";
                    return response;
                }

                var orderDTOs = orders.Select(order => new OrderOverviewResponseDTO
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    TotalPrice = order.TotalPrice,
                    ProductVariantId = order.OrderItems.First().ProductVariantId,
                    ProductImageUrl = order.OrderItems.First().ProductVariant.Product.ImageUrl,
                    ProductSummary = order.OrderItems.Count > 1
                        ? $"{ order.OrderItems.First().ProductVariant.Product.Title} and {order.OrderItems.Count - 1} more..."
                        : order.OrderItems.First().ProductVariant.Product.Title
                }).ToList();

                response.Data = orderDTOs;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving orders: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder(int userId)
		{
            var response = new ServiceResponse<bool>();

            try
            {
                var cartResponse = await _cartService.GetCartDetailsAsync(userId);
                if (!cartResponse.Success)
                {
                    response.Message = "Unable to retrieve cart details.";
                    return response;
                }

                var cartDetails = cartResponse.Data;
                var orderItems = cartDetails.Products.Select(p => new OrderItem
                {
                    ProductVariantId = p.ProductVariantId,
                    Quantity = p.Quantity,
                    TotalPrice = p.Quantity * p.Price
                }).ToList();

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = orderItems.Sum(item => item.TotalPrice),
                    OrderItems = orderItems
                };

                _context.Orders.Add(order);

                foreach ( var item in orderItems)
                {
                    var removeResponse = await _cartService.RemoveProductFromCartAsync(userId, item.ProductVariantId);

                    if (!removeResponse.Success)
                    {
                        response.Success = false;
                        response.Message = removeResponse.Message;
                        return response;
                    }
                }

                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error placing order: {ex.Message}";
            }

            return response;
        }
	}
}
