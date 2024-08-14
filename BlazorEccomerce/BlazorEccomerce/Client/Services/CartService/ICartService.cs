namespace BlazorEccomerce.Client.Services.CartService
{
	public interface ICartService
	{
		event Action OnChange;
		Task AddToCart(CartItemDTO cartItem);
		Task<List<CartItemDTO>> GetCartItems();
		Task<List<CartProductResponseDTO>> GetCartProducts();
		Task RemoveProductFromCart(int productVariantId);
        Task UpdateCartForUserAsync(int userId, List<CartItemDTO> cartItemsDTO);
	}
}
