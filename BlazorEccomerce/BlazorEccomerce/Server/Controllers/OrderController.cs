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

		[HttpGet("{userId}")]
		public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponseDTO>>>> GetOrders(int userId)
		{
			var result = await _orderService.GetOrders(userId);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}
			return Ok(result);
		}

		[HttpGet("details/{userId}/{orderId}")]
		public async Task<ActionResult<ServiceResponse<OrderDetailsResponseDTO>>> GetOrderDetails (int userId, int orderId)
		{
			var result = await _orderService.GetOrderDetails(userId, orderId);
			return Ok(result);
		}
    }
}
