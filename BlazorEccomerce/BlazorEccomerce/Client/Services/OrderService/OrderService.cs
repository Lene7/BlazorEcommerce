
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace BlazorEccomerce.Client.Services.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly HttpClient _httpClient;
		private readonly AuthenticationStateProvider _authStateProvider;
		private readonly NavigationManager _navigationManager;

		public OrderService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, NavigationManager navigationManager)
        {
			_httpClient = httpClient;
			_authStateProvider = authStateProvider;
			_navigationManager = navigationManager;
        }

        public async Task<List<OrderOverviewResponseDTO>> GetOrders()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var userIdClaim = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                _navigationManager.NavigateTo("login");
                return new List<OrderOverviewResponseDTO>();  
            }

            try
            {
                Console.WriteLine($"Fetching orders for userId: {userId}");

                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponseDTO>>>($"api/order/{userId}");

                return result?.Data ?? new List<OrderOverviewResponseDTO>(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving orders: {ex.Message}");
                return new List<OrderOverviewResponseDTO>(); 
            }
        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
		{
			var authState = await _authStateProvider.GetAuthenticationStateAsync();
			var userIdClaim = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
			{
				_navigationManager.NavigateTo("login");
				return new ServiceResponse<bool> { Success = false, Message = "User not authenticated." };
			}

			try
			{
                Console.WriteLine($"Placing order for userId: {userId}");

                var response = await _httpClient.PostAsJsonAsync($"api/order?userId={userId}", new { });

                if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
				}
				else
				{
					var errorResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
					return new ServiceResponse<bool> { Success = false, Message = errorResponse?.Message };
				}
			}
			catch (Exception ex)
			{
				return new ServiceResponse<bool> { Success = false, Message = $"Error: {ex.Message}" };
			}
		}
	}
}
