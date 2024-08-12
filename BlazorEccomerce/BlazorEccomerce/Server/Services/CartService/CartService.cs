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

			var productVariantIds = cartItemsDTO.Select(dto => dto.ProductVariantId).Distinct();
			var productVariants = await _context.ProductVariants
				.Include(v => v.Product)
				.Include(v => v.ProductType)
				.Where(v => productVariantIds.Contains(v.ProductVariantId))
				.ToListAsync();

			foreach (var dto in cartItemsDTO)
			{
				var productVariant = productVariants
					.FirstOrDefault(v => v.ProductVariantId == dto.ProductVariantId);

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

            // Fetch cart items for the user including related ProductVariant, Product, and ProductType
            var cartItems = await _context.CartItems
                .Include(ci => ci.ProductVariant)
                .ThenInclude(pv => pv.ProductId)
                .Include(ci => ci.ProductVariant)
                .ThenInclude(pv => pv.ProductType)
                .Where(ci => ci.Cart.UserId == userId)
                .ToListAsync();

            // Group cart items by ProductVariant
            var productVariants = cartItems
                .Select(ci => ci.ProductVariant)
                .Distinct()
                .ToList();

            foreach (var item in cartItems)
            {
                var productVariant = productVariants
                    .FirstOrDefault(v => v.ProductId == item.ProductVariant.ProductId && v.ProductTypeId == item.ProductVariant.ProductTypeId);

                if (productVariant != null && productVariant.Product != null && productVariant.ProductType != null)
                {
					var cartProduct = new CartProductResponseDTO
					{
						CartItemId = item.Id,
						ProductId = productVariant.ProductId,
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

		public async Task<ServiceResponse<bool>> RemoveProductFromCartAsync(int userId, int cartItemId)
		{
			var response = new ServiceResponse<bool> { Data = false };
			Console.WriteLine($"Removing item. UserId: {userId}, CartItemId: {cartItemId}");
			var cart = await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (cart == null)
			{
				response.Message = "Cart not found.";
				return response;
			}

			var itemToRemove = cart.CartItems
				.FirstOrDefault(ci => ci.Id == cartItemId); 

			if (itemToRemove == null)
			{
				response.Message = $"Item with Id {cartItemId} not found in cart.";
				Console.WriteLine(response.Message);
				return response;
			}

			_context.CartItems.Remove(itemToRemove);
			await _context.SaveChangesAsync();
			response.Data = true;

			return response;
		}
	}
}
