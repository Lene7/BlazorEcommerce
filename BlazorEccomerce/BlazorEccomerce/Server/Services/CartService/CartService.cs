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

		public async Task<ServiceResponse<CartDetailDTO>> GetCartDetailsAsync(int userId)
		{
			var response = new ServiceResponse<CartDetailDTO>
			{
				Data = new CartDetailDTO
				{
					Products = new List<CartProductResponseDTO>()
				}
			};

			var cart = await _context.Carts
				.Include(c => c.CartItems)
					.ThenInclude(ci => ci.ProductVariant)
						.ThenInclude(pv => pv.Product)
				.Include(c => c.CartItems)
					.ThenInclude(ci => ci.ProductVariant)
						.ThenInclude(pv => pv.ProductType)
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (cart == null)
			{
				response.Message = "Cart not found.";
				response.Success = false;
				return response;
			}

			foreach (var cartItem in cart.CartItems)
			{
				var productVariant = cartItem.ProductVariant;

				if (productVariant == null || productVariant.Product == null || productVariant.ProductType == null)
				{
					continue;
				}

				var cartProduct = new CartProductResponseDTO
				{
					CartItemId = cartItem.Id,
					ProductId = productVariant.Product.Id,
					Title = productVariant.Product.Title,
					ImageUrl = productVariant.Product.ImageUrl,
					Price = productVariant.Price,
					ProductType = productVariant.ProductType.Name,
					ProductTypeId = productVariant.ProductTypeId,
					Quantity = cartItem.Quantity,
					ProductVariantId = productVariant.ProductVariantId
				};

				response.Data.Products.Add(cartProduct);
			}

			response.Success = response.Data.Products.Any();
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

            // Fetch cart items for the user including related ProductVariant, Product, and ProductType
            var cartItems = await _context.CartItems
                .Include(ci => ci.ProductVariant)
                .ThenInclude(pv => pv.Product)
                .Include(ci => ci.ProductVariant)
                .ThenInclude(pv => pv.ProductType)
                .Where(ci => ci.Cart.UserId == userId)
                .ToListAsync();

            // Group cart items by ProductVariant
            var productVariants = cartItems
                .Select(ci => ci.ProductVariant)
                .Distinct()
                .ToList();

			Console.WriteLine("Fetched cart items:");
			foreach (var item in cartItems)
			{
				Console.WriteLine($"CartItemId: {item.Id}, ProductId: {item.ProductVariant?.Product?.Id}, ProductVariantId: {item.ProductVariant?.ProductVariantId}");
			}

			foreach (var item in cartItems)
            {
                var productVariant = item.ProductVariant;

				if (productVariant == null)
				{
					Console.Error.WriteLine($"ProductVariant is null for CartItemId: {item.Id}");
				}
				else if (productVariant.Product == null)
				{
					Console.Error.WriteLine($"Product is null for ProductVariantId: {productVariant.ProductVariantId}");
				}
				else if (productVariant.ProductType == null)
				{
					Console.Error.WriteLine($"ProductType is null for ProductTypeId: {productVariant.ProductTypeId}");
				}
				else
				{
					var cartProduct = new CartProductResponseDTO
					{
						CartItemId = item.Id,
						ProductId = productVariant.Product.Id,
						Title = productVariant.Product.Title,
						ImageUrl = productVariant.Product.ImageUrl,
						Price = productVariant.Price,
						ProductType = productVariant.ProductType.Name,
						ProductTypeId = productVariant.ProductTypeId,
						Quantity = item.Quantity,
						ProductVariantId = productVariant.ProductVariantId
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
				//var itemsToRemove = existingCart.CartItems.Where(ci => !newProductIds.Contains(ci.ProductId)).ToList();
				
				//_context.CartItems.RemoveRange(itemsToRemove);

				foreach (var dto in cartItemsDTO)
				{
					var existingItem = existingCart.CartItems
						.FirstOrDefault(ci => ci.ProductId == dto.ProductId);

					if (existingItem != null)
					{
						existingItem.Quantity = dto.Quantity;
						existingItem.Price = dto.Price;
						_context.Entry(existingItem).State = EntityState.Modified;
					}
					else
					{
						var newItem = new CartItem
						{
							ProductVariantId = dto.ProductVariantId,
							ProductId = dto.ProductId,
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
		}

		// New AddToCart method
		public async Task<ServiceResponse<bool>> AddToCartAsync(int userId, BlazorEccomerce.Shared.CartItemDTO cartItemDTO)
		{
			var response = new ServiceResponse<bool> { Data = false };

			var cartIdResponse = await GetCartIdForUserAsync(userId);
			if (!cartIdResponse.Success)
			{
				response.Message = cartIdResponse.Message;
				return response;
			}

			var cartId = cartIdResponse.Data;

			var existingItem = await _context.CartItems
				.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductVariantId == cartItemDTO.ProductVariantId);

			if (existingItem != null)
			{
				existingItem.Quantity += cartItemDTO.Quantity;
				existingItem.Price = cartItemDTO.Price;
				_context.Entry(existingItem).State = EntityState.Modified;
			}
			else
			{
				var newItem = new CartItem
				{
					ProductVariantId = cartItemDTO.ProductVariantId,
					ProductId = cartItemDTO.ProductId,
					Quantity = cartItemDTO.Quantity,
					Price = cartItemDTO.Price,
					CartId = cartId
				};
				_context.CartItems.Add(newItem);
			}

			await _context.SaveChangesAsync();
			response.Data = true;
			return response;
		}


		public async Task<ServiceResponse<int>> GetCartIdForUserAsync(int userId)
		{
			var response = new ServiceResponse<int> { Data = 0 };

			var cart = await _context.Carts
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (cart == null)
			{
				// Create a new cart if it doesn't exist
				cart = new BlazorEccomerce.Shared.Cart
				{
					UserId = userId
				};
				_context.Carts.Add(cart);
				await _context.SaveChangesAsync();
			}

			response.Data = cart.Id;
			response.Success = true;
			return response;
		}

		public async Task<ServiceResponse<bool>> RemoveProductFromCartAsync(int userId, int productVariantId)
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

			var itemsToRemove = cart.CartItems
				.Where(ci => ci.ProductVariantId ==  productVariantId)
				.ToList();

			if (!itemsToRemove.Any())
			{
				response.Message = $"No items found with ProductVariantId {productVariantId}";
				return response;
			}

			_context.CartItems.RemoveRange(itemsToRemove);
			await _context.SaveChangesAsync();
			response.Data = true;

			return response;
		}
	}
}
