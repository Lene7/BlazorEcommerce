using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEccomerce.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder([FromQuery] int userId)
		{
			var result = await _orderService.PlaceOrder(userId);
			return Ok(result);
		}
    }
}
