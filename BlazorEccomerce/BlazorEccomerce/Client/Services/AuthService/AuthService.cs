
using Blazored.LocalStorage;

namespace BlazorEccomerce.Client.Services.AuthService
{
	public class AuthService : IAuthService
	{
		private readonly HttpClient _http;
		private readonly ILocalStorageService _localStorage;

		public AuthService(HttpClient http, ILocalStorageService localStorage)
        {
			_http = http;
			_localStorage = localStorage;
        }

		public async Task<ServiceResponse<bool>> ChangePassword(string userId, string currentPassword, string newPassword)
		{
			var request = new UserChangePassword
			{
				CurrentPassword = currentPassword,
				NewPassword = newPassword,
				ConfirmNewPassword = newPassword 
			};

			var response = await _http.PostAsJsonAsync($"api/auth/change-password/{userId}", request);

			return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
		}

		public async Task<ServiceResponse<string>> Login(UserLogin request)
		{
			var result = await _http.PostAsJsonAsync("api/auth/login", request);
			if (result.IsSuccessStatusCode)
			{
				var response = await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();

				if (response != null && response.Data != null)
				{
					await _localStorage.SetItemAsync("token", response.Data); // Store the token in local storage
				}

				return response;
			}

			return new ServiceResponse<string>
			{
				Success = false,
				Message = "Login failed"
			};
		}

		public async Task<ServiceResponse<int>> Register(UserRegister request)
		{
			var result = await _http.PostAsJsonAsync("api/auth/register", request);
			return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
		}
	}
}
