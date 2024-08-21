namespace BlazorEccomerce.Client.Services.OrderService
{
	public interface IOrderService
	{
		Task<ServiceResponse<bool>> PlaceOrder();
	}
}
