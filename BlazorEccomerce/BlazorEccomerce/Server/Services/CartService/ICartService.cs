namespace BlazorEccomerce.Server.Services.CartService
{
	public interface ICartService
	{
		Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartProducts(List<BlazorEccomerce.Shared.CartItemDTO> cartItemsDTO);
		Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartForUserAsync(int userId);
		Task<ServiceResponse<bool>> UpdateCartForUserAsync(int userId, List<BlazorEccomerce.Shared.CartItemDTO> cartItemsDTO);
		Task<ServiceResponse<bool>> AddToCartAsync(int userId, CartItemDTO cartItemDTO);
		Task<ServiceResponse<Cart>> GetCartByUserIdAsync(int userId);
		Task<ServiceResponse<bool>> RemoveProductFromCartAsync(int userId, int productId, int productTypeId);
	}
}
