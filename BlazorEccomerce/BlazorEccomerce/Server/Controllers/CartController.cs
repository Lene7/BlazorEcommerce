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
            var cartIdResponse = await _cartService.GetCartIdForUserAsync(userId);
            if (!cartIdResponse.Success)
            {
                return BadRequest(cartIdResponse.Message);
            }

            var cartItem = new CartItem
            {
                CartId = cartIdResponse.Data,
                ProductVariantId = cartItemDTO.ProductVariantId,
                Quantity = cartItemDTO.Quantity,
                Price = cartItemDTO.Price
            };

            _context.CartItems.Add(cartItem);
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
                ProductId = item.ProductVariant.ProductId,
                Title = item.ProductVariant.Product.Title,
                ProductType = item.ProductVariant.ProductType.Name,
                ImageUrl = item.ProductVariant.Product.ImageUrl,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList();

            return Ok(new ServiceResponse<List<CartProductResponseDTO>> { Data = cartProductResponse });
        }

		[HttpPut("update/{userId}")]
		public async Task<ActionResult<ServiceResponse<bool>>> UpdateUserCart(int userId, [FromBody] List<CartItemDTO> cartItemsDTO)
		{
			if (userId <= 0)
			{
				return BadRequest("Invalid user ID");
			}

			if (cartItemsDTO == null || !cartItemsDTO.Any())
			{
				return BadRequest("Cart items are required.");
			}

			var result = await _cartService.UpdateCartForUserAsync(userId, cartItemsDTO);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result);
		}

		[HttpDelete("remove/{userId}/{productId}/{productTypeId}")]
        public async Task<IActionResult> RemoveProductFromCart(int userId, int productVariantId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID");
            }

            var result = await _cartService.RemoveProductFromCartAsync(userId, productVariantId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }
    }
}
