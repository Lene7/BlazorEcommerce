namespace BlazorEccomerce.Client.Services.CartService
{
	public interface ICartService
	{
		event Action OnChange;
		Task<ServiceResponse<CartDetailDTO>> GetCartDetails();
		Task AddToCart(CartItemDTO cartItem);
		Task RemoveProductFromCart(int productVariantId);
        Task UpdateCartForUserAsync(int userId, List<CartItemDTO> cartItemsDTO);
	}
}
