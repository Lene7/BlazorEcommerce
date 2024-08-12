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
		private readonly DataContext _context;

		public CartController(DataContext context,ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
			_productService = productService;
			_context = context;
		}

		[HttpPost("add/{userId}")]
		public async Task<IActionResult> AddToCart(int userId, [FromBody] CartItemDTO cartItemDTO)
		{
			// Ensure the correct ProductId and ProductVariantId are passed
			Console.WriteLine($"ProductId: {cartItemDTO.ProductId}, ProductVariantId: {cartItemDTO.ProductVariantId}");

			var productExists = await _context.Products.AnyAsync(p => p.Id == cartItemDTO.ProductId);
			if (!productExists)
			{
				Console.WriteLine($"Product with Id {cartItemDTO.ProductId} does not exist.");
				return BadRequest("The product does not exist.");
			}
			else
			{
				Console.WriteLine($"Product with Id {cartItemDTO.ProductId} exists.");
			}

			
			var cartIdResponse = await _cartService.GetCartIdForUserAsync(userId);
			if (!cartIdResponse.Success)
			{
				return BadRequest(cartIdResponse.Message);
			}

			var cartId = cartIdResponse.Data;

			var existingItem = await _context.CartItems
				.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductVariantId == cartItemDTO.ProductVariantId);

			if (existingItem != null)
			{
				existingItem.Quantity += cartItemDTO.Quantity;
				existingItem.Price = cartItemDTO.Price;
				_context.Entry(existingItem).State = EntityState.Modified;
			}
			else
			{
				var cartItem = new CartItem
				{
					CartId = cartId,
					ProductId = cartItemDTO.ProductId,
					ProductVariantId = cartItemDTO.ProductVariantId,
					Quantity = cartItemDTO.Quantity,
					Price = cartItemDTO.Price
				};

				_context.CartItems.Add(cartItem);
			}

			await _context.SaveChangesAsync();

			return Ok();
		}

		[HttpGet("items/{userId}")]
        public async Task<ActionResult<ServiceResponse<List<CartItemDTO>>>> GetCartItems(int userId)
        {
            var result = await _cartService.GetCartIdForUserAsync(userId);
            if (result.Success)
            {
                // Fetch cart items using the cart ID
                var cartItems = await _context.CartItems
                    .Where(ci => ci.CartId == result.Data)
					.Include(ci => ci.ProductVariant)
			        .ThenInclude(pv => pv.Product)
			        .Include(ci => ci.ProductVariant)
			        .ThenInclude(pv => pv.ProductType)
					.ToListAsync();

                var cartItemDTOs = cartItems.Select(ci => new CartItemDTO
                {
                    ProductVariantId = ci.ProductVariantId,
					ProductId = ci.ProductId,
					ProductTypeId = ci.ProductVariant?.ProductTypeId ?? default,
					Quantity = ci.Quantity,
                    Price = ci.Price
                }).ToList();

                return Ok(new ServiceResponse<List<CartItemDTO>> { Data = cartItemDTOs });
            }
            return NotFound(result.Message);
        }

        [HttpGet("products/{userId}")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDTO>>>> GetCartProducts(int userId)
        {
            var cartIdResult = await _cartService.GetCartIdForUserAsync(userId);

            if (!cartIdResult.Success)
            {
                return NotFound("Cart not found");
            }

            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartIdResult.Data)
                .Include(ci => ci.ProductVariant)
                .ThenInclude(pv => pv.Product)
                .Include(ci => ci.ProductVariant)
                .ThenInclude(pv => pv.ProductType)
                .ToListAsync();

            var cartProductResponse = cartItems.Select(item => new CartProductResponseDTO
            {
                ProductId = item.ProductId,
                Title = item.ProductVariant.Product.Title,
                ProductType = item.ProductVariant.ProductType.Name,
                ImageUrl = item.ProductVariant.Product.ImageUrl,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList();

            return Ok(new ServiceResponse<List<CartProductResponseDTO>> { Data = cartProductResponse });
        }

		[HttpPut("update/{userId}")]
		public async Task<IActionResult> UpdateCartForUserAsync(int userId, [FromBody] List<CartItemDTO> cartItemsDTO)
		{
			try
			{
				var result = await _cartService.UpdateCartForUserAsync(userId, cartItemsDTO);
				if (result.Data)
				{
					return Ok();
				}
				else
				{
					return BadRequest("Failed to update cart.");
				}
			}
			catch (Exception ex)
			{
				var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
				return StatusCode(500, $"Internal server error: {errorMessage}");
			}
		}

		[HttpDelete("delete/{userId}/{cartItemId}")]
		public async Task<IActionResult> DeleteCartItemAsync(int userId, int cartItemId)
		{
			try
			{
				var result = await _cartService.RemoveProductFromCartAsync(userId, cartItemId); // Pass userId
				if (result.Data)
				{
					return Ok();
				}
				else
				{
					return NotFound("Cart item not found or could not be deleted.");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
