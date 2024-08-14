
using BlazorEccomerce.Shared;
using Blazored.LocalStorage;
using System.Security.Claims;

namespace BlazorEccomerce.Client.Services.CartService
{
	public class CartService : ICartService
	{
		private readonly HttpClient _http;
		private readonly AuthenticationStateProvider _authStateProvider;

		public CartService(HttpClient http, AuthenticationStateProvider authStateProvider)
		{
			_http = http;
			_authStateProvider = authStateProvider;
		}
		
		public event Action OnChange;

		public async Task<ServiceResponse<CartDetailDTO>> GetCartDetails()
		{
			try
			{
				var authState = await _authStateProvider.GetAuthenticationStateAsync();
				var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					throw new Exception("User not authenticated");
				}

				var response = await _http.GetAsync($"api/cart/details/{userId}");
				response.EnsureSuccessStatusCode();

				var result = await response.Content.ReadFromJsonAsync<ServiceResponse<CartDetailDTO>>();
				return result;
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error fetching cart details: {ex.Message}");
				return new ServiceResponse<CartDetailDTO> { Success = false, Message = ex.Message };
			}
		}

		public async Task AddToCart(CartItemDTO cartItem)
		{
			try
			{
				var authState = await _authStateProvider.GetAuthenticationStateAsync();
				var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					throw new Exception("User not authenticated");
				}
				Console.WriteLine($"Adding to cart - ProductVariantId: {cartItem.ProductVariantId}, ProductTypeId: {cartItem.ProductTypeId}, ProductId: {cartItem.ProductId}, Quantity: {cartItem.Quantity}, Price: {cartItem.Price}");
				var cartDetailResponse = await GetCartDetails();
                var cartDetails = cartDetailResponse.Data;

                if (cartDetails == null)
                {
                    throw new Exception("Failed to load cart details.");
                }

                var existingItem = cartDetails.Products.FirstOrDefault(item => item.ProductVariantId == cartItem.ProductVariantId);
                if (existingItem != null)
				{
					existingItem.Quantity += cartItem.Quantity;

                    var updatedCartItems = cartDetails.Products.Select(p => new CartItemDTO
                    {
                        ProductVariantId = p.ProductVariantId,
                        ProductId = p.ProductId,
                        Quantity = p.Quantity,
                        Price = p.Price
                    }).ToList();

                    await UpdateCartForUserAsync(int.Parse(userId), updatedCartItems);
                }
				else
				{
					var response = await _http.PostAsJsonAsync($"api/cart/add/{userId}", cartItem);
					response.EnsureSuccessStatusCode();
				}
				OnChange?.Invoke();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error adding item to cart: {ex.Message}");
			}
		}

		public async Task RemoveProductFromCart(int productVariantId)
		{
			try
			{
				var authState = await _authStateProvider.GetAuthenticationStateAsync();
				var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					throw new Exception("User not authenticated");
				}

				Console.WriteLine($"Attempting to remove item with ProductVariantId: {productVariantId}");
				var url = $"api/cart/delete/{userId}/{productVariantId}";
				var response = await _http.DeleteAsync(url);
				response.EnsureSuccessStatusCode();
				OnChange?.Invoke();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error removing item from cart: {ex.Message}");
			}
		}

		public async Task UpdateCartForUserAsync(int userId, List<CartItemDTO> cartItemsDTO)
		{
			try
			{
				var response = await _http.PutAsJsonAsync($"api/cart/update/{userId}", cartItemsDTO);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error updating cart: {ex.Message}");
			}
		}
    }
}
