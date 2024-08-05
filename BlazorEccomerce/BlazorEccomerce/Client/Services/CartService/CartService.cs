
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

				var response = await _http.PostAsJsonAsync($"api/cart/add/{userId}", cartItem);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error adding item to cart: {ex.Message}");
			}
		}

		public async Task<List<CartItemDTO>> GetCartItems()
		{
			try
			{
				var authState = await _authStateProvider.GetAuthenticationStateAsync();
				var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					throw new Exception("User not authenticated");
				}

				var response = await _http.GetAsync($"api/cart/items/{userId}");
				response.EnsureSuccessStatusCode();

				var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartItemDTO>>>();
				return result.Data;
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error fetching cart items: {ex.Message}");
				return new List<CartItemDTO>();
			}
		}

		public async Task<List<CartProductResponseDTO>> GetCartProducts()
		{
			try
			{
				var authState = await _authStateProvider.GetAuthenticationStateAsync();
				var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					throw new Exception("User not authenticated");
				}

				var response = await _http.GetAsync($"api/cart/products/{userId}");
				response.EnsureSuccessStatusCode();

				var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponseDTO>>>();
				return result.Data;
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error fetching cart products: {ex.Message}");
				return new List<CartProductResponseDTO>();
			}
		}

		public async Task RemoveProductFromCart(int productId, int productTypeId)
		{
			try
			{
				var authState = await _authStateProvider.GetAuthenticationStateAsync();
				var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					throw new Exception("User not authenticated");
				}

				var response = await _http.DeleteAsync($"api/cart/remove/{userId}/{productId}/{productTypeId}");
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error removing item from cart: {ex.Message}");
			}
		}

		public async Task UpdateQuantity(CartItemDTO cartItem)
		{
			try
			{
				var authState = await _authStateProvider.GetAuthenticationStateAsync();
				var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					throw new Exception("User not authenticated");
				}

				var response = await _http.PutAsJsonAsync($"api/cart/update/{userId}", cartItem);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error updating item quantity: {ex.Message}");
			}
		}
	}
}
