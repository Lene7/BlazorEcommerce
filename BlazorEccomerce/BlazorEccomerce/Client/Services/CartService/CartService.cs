
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

				// Fetch current cart items
				var currentItems = await GetCartItems();
				var existingItem = currentItems.FirstOrDefault(item => item.ProductVariantId == cartItem.ProductVariantId);

				if (existingItem != null)
				{
					// Update quantity if item already exists
					existingItem.Quantity += cartItem.Quantity;
					await UpdateCartForUserAsync(int.Parse(userId), currentItems);
				}
				else
				{
					// Add new item if it does not exist
					var response = await _http.PostAsJsonAsync($"api/cart/add/{userId}", cartItem);
					response.EnsureSuccessStatusCode();
				}
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

        public async Task UpdateQuantity(List<CartItemDTO> cartItem)
        {
			try
			{
				var authState = await _authStateProvider.GetAuthenticationStateAsync();
				var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					throw new Exception("User not authenticated");
				}

				var response = await _http.PutAsJsonAsync($"api/cart/update/{userId}", cartItem); // Use cartItem here
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error updating item quantity: {ex.Message}");
			}
		}
    }
}
