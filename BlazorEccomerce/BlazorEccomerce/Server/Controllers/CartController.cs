using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorEccomerce.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService;
		private readonly IProductService _productService;

		public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
			_productService = productService;
		}

		[HttpPost("add/{userId}")]
		public async Task<IActionResult> AddToCart(int userId, [FromBody] CartItemDTO cartItem)
		{
			if (userId <= 0)
			{
				return BadRequest("Invalid user ID");
			}

			var result = await _cartService.AddToCartAsync(userId, cartItem);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok();
		}

		[HttpGet("items/{userId}")]
		public async Task<ActionResult<ServiceResponse<List<CartItemDTO>>>> GetCartItems(int userId)
		{
			var result = await _cartService.GetCartByUserIdAsync(userId);
			if (result.Success)
			{
				return Ok(result);
			}
			return NotFound(result.Message);
		}

		[HttpGet("products/{userId}")]
		public async Task<ActionResult<ServiceResponse<List<CartProductResponseDTO>>>> GetCartProducts(int userId)
		{
			var cartResult = await _cartService.GetCartByUserIdAsync(userId);

			if (!cartResult.Success || cartResult.Data == null)
			{
				return NotFound("Cart not found");
			}

			var cartItems = cartResult.Data.CartItems; 
			var cartProductResponse = new List<CartProductResponseDTO>();

			foreach (var item in cartItems)
			{
				var productResult = await _productService.GetProductAsync(item.ProductId);
				if (productResult.Success && productResult.Data != null)
				{
					var product = productResult.Data;

					cartProductResponse.Add(new CartProductResponseDTO
					{
						ProductId = item.ProductId,
						Title = product.Title,
						ProductType = "Unknown", 
						ImageUrl = product.ImageUrl,
						Price = item.Price,
						Quantity = item.Quantity
					});
				}
			}

			return Ok(new ServiceResponse<List<CartProductResponseDTO>> { Data = cartProductResponse });
		}

		[HttpPost("update/{userId}")]
		public async Task<ActionResult<ServiceResponse<bool>>> UpdateUserCart(int userId, [FromBody] List<CartItemDTO> cartItems)
		{
			if (userId <= 0)
			{
				return BadRequest("Invalid user ID");
			}

			var result = await _cartService.UpdateCartForUserAsync(userId, cartItems);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result);
		}

		[HttpDelete("remove/{userId}/{productId}/{productTypeId}")]
		public async Task<IActionResult> RemoveProductFromCart(int userId, int productId, int productTypeId)
		{
			if (userId <= 0)
			{
				return BadRequest("Invalid user ID");
			}

			var result = await _cartService.RemoveProductFromCartAsync(userId, productId, productTypeId);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok();
		}
	}
}
