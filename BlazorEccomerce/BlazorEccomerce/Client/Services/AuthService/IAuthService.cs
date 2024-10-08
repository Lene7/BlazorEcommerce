﻿namespace BlazorEccomerce.Client.Services.AuthService
{
	public interface IAuthService
	{
		Task<ServiceResponse<int>> Register(UserRegister request);
		Task<ServiceResponse<string>> Login (UserLogin request);
		Task<ServiceResponse<bool>> ChangePassword(string userId, string currentPassword, string newPassword);
	}
}
