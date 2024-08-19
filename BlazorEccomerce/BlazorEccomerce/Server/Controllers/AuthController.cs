using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorEccomerce.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
        {
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
		{
			var response = await _authService.Register(
				new User 
			{ 
				Email = request.Email
			},
			request.Password);

			if(!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}

		[HttpPost("login")]
		public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
		{
			var response = await _authService.Login(request.Email, request.Password);
			if(!response.Success)
			{
				return BadRequest(response);
			}

			return response;
		}

		[HttpPost("change-password/{userId}")]
		public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword(string userId, UserChangePassword request)
		{
			var result = await _authService.ChangePassword(userId, request.CurrentPassword, request.NewPassword);

			if (result.Success)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result.Message);
			}
		}
	}
}
