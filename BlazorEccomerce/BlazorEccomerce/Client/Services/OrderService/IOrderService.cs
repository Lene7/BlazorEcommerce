namespace BlazorEccomerce.Client.Services.OrderService
{
	public interface IOrderService
	{
		Task<ServiceResponse<bool>> PlaceOrder();
		Task<List<OrderOverviewResponseDTO>> GetOrders();
		Task<OrderDetailsResponseDTO> GetOrderDetails(int userId, int orderId);
	}
}
