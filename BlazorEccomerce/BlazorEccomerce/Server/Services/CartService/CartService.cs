using BlazorEccomerce.Shared;
using BlazorEccomerce.Client.Pages;
using BlazorEccomerce.Server.Migrations;

namespace BlazorEccomerce.Server.Services.CartService
{
	public class CartService : ICartService
	{
		private readonly DataContext _context;

		public CartService(DataContext context)
        {
            _context = context;
        }


		public async Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartProducts(List<BlazorEccomerce.Shared.CartItemDTO> cartItemsDTO)
		{
			var response = new ServiceResponse<List<CartProductResponseDTO>>
			{
				Data = new List<CartProductResponseDTO>()
			};

			if (cartItemsDTO == null || !cartItemsDTO.Any())
			{
				response.Message = "Cart items are required.";
				response.Success = false;
				return response;
			}

			var productIds = cartItemsDTO.Select(dto => dto.ProductId).Distinct();
			var productVariants = await _context.ProductVariants
				.Include(v => v.Product)
				.Include(v => v.ProductType)
				.Where(v => productIds.Contains(v.ProductId))
				.ToListAsync();

			foreach (var dto in cartItemsDTO)
			{
				var productVariant = productVariants
					.FirstOrDefault(v => v.ProductId == dto.ProductId && v.ProductTypeId == dto.ProductTypeId);

				if (productVariant != null)
				{
					if (productVariant.Product != null && productVariant.ProductType != null)
					{
						var cartProduct = new CartProductResponseDTO
						{
							ProductId = productVariant.ProductId,
							Title = productVariant.Product.Title,
							ImageUrl = productVariant.Product.ImageUrl,
							Price = productVariant.Price,
							ProductType = productVariant.ProductType.Name,
							ProductTypeId = productVariant.ProductTypeId,
							Quantity = dto.Quantity
						};

						response.Data.Add(cartProduct);
					}
					else
					{
						Console.Error.WriteLine("Product or ProductType is null.");
					}
				}
				else
				{
					Console.Error.WriteLine("ProductVariant is null.");
				}
			}

			response.Success = response.Data.Any();
			if (!response.Success)
			{
				response.Message = "No valid products found.";
			}

			return response;
		}

		public async Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartForUserAsync(int userId)
		{
			var response = new ServiceResponse<List<CartProductResponseDTO>>
			{
				Data = new List<CartProductResponseDTO>()
			};

			var cartItems = await _context.CartItems
			   .Where(ci => ci.Cart.UserId == userId)
			   .ToListAsync();

			var productIds = cartItems.Select(ci => ci.ProductId).Distinct();
			var productVariants = await _context.ProductVariants
				.Where(v => productIds.Contains(v.ProductId))
				.Include(v => v.Product)
				.Include(v => v.ProductType)
				.ToListAsync();

			foreach (var item in cartItems)
			{
				var productVariant = productVariants
					.FirstOrDefault(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId);

				if (productVariant != null && productVariant.Product != null && productVariant.ProductType != null)
				{
					var cartProduct = new CartProductResponseDTO
					{
						ProductId = productVariant.ProductId,
						Title = productVariant.Product.Title,
						ImageUrl = productVariant.Product.ImageUrl,
						Price = productVariant.Price,
						ProductType = productVariant.ProductType.Name,
						ProductTypeId = productVariant.ProductTypeId,
						Quantity = item.Quantity
					};

					response.Data.Add(cartProduct);
				}
			}

			response.Success = response.Data.Any();
			if (!response.Success)
			{
				response.Message = "No valid products found.";
			}

			return response;
		}

		public async Task<ServiceResponse<bool>> UpdateCartForUserAsync(int userId, List<BlazorEccomerce.Shared.CartItemDTO> cartItemsDTO)
		{
			var response = new ServiceResponse<bool> { Data = false };

			var existingCart = await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (existingCart == null)
			{
				response.Message = "Cart not found for user.";
				return response;
			}

			var newProductIds = cartItemsDTO.Select(dto => dto.ProductId).Distinct();
			var itemsToRemove = existingCart.CartItems
				.Where(ci => !newProductIds.Contains(ci.ProductId))
				.ToList();

			_context.CartItems.RemoveRange(itemsToRemove);

			foreach (var dto in cartItemsDTO)
			{
				var existingItem = existingCart.CartItems
					.FirstOrDefault(ci => ci.ProductId == dto.ProductId && ci.ProductTypeId == dto.ProductTypeId);

				if (existingItem != null)
				{
					existingItem.Quantity = dto.Quantity;
					existingItem.Price = dto.Price;
				}
				else
				{
					var newItem = new CartItem
					{
						ProductId = dto.ProductId,
						ProductTypeId = dto.ProductTypeId,
						Quantity = dto.Quantity,
						Price = dto.Price,
						CartId = existingCart.Id
					};
					_context.CartItems.Add(newItem);
				}
			}

			await _context.SaveChangesAsync();
			response.Data = true;
			return response;
		}

		// New AddToCart method
		public async Task<ServiceResponse<bool>> AddToCartAsync(int userId, BlazorEccomerce.Shared.CartItemDTO cartItemDTO)
		{
			var response = new ServiceResponse<bool> { Data = false };

			var existingCart = await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (existingCart == null)
			{
				response.Message = "Cart not found for user.";
				return response;
			}

			var existingItem = existingCart.CartItems
				.FirstOrDefault(ci => ci.ProductId == cartItemDTO.ProductId && ci.ProductTypeId == cartItemDTO.ProductTypeId);

			if (existingItem != null)
			{
				existingItem.Quantity += cartItemDTO.Quantity;
				existingItem.Price = cartItemDTO.Price;
			}
			else
			{
				var newItem = new CartItem
				{
					ProductId = cartItemDTO.ProductId,
					ProductTypeId = cartItemDTO.ProductTypeId,
					Quantity = cartItemDTO.Quantity,
					Price = cartItemDTO.Price,
					CartId = existingCart.Id
				};
				_context.CartItems.Add(newItem);
			}

			await _context.SaveChangesAsync();
			response.Data = true;
			return response;
		}


		public async Task<ServiceResponse<BlazorEccomerce.Shared.Cart>> GetCartByUserIdAsync(int userId)
		{
			var response = new ServiceResponse<BlazorEccomerce.Shared.Cart> { Data = null };

			var cart = await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (cart == null)
			{
				response.Message = "Cart not found for user.";
				return response;
			}

			response.Data = cart;
			response.Success = true;
			return response;
		}

		public async Task<ServiceResponse<bool>> RemoveProductFromCartAsync(int userId, int productId, int productTypeId)
		{
			var response = new ServiceResponse<bool> { Data = false };

			var cart = await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (cart == null)
			{
				response.Message = "Cart not found.";
				return response;
			}

			var itemToRemove = cart.CartItems
				.FirstOrDefault(ci => ci.ProductId == productId && ci.ProductTypeId == productTypeId);

			if (itemToRemove == null)
			{
				response.Message = "Item not found in cart.";
				return response;
			}

			_context.CartItems.Remove(itemToRemove);
			await _context.SaveChangesAsync();
			response.Data = true;

			return response;
		}
	}
}
