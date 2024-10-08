﻿namespace BlazorEccomerce.Server.Services.OrderService
{
	public interface IOrderService
	{
		Task<ServiceResponse<bool>> PlaceOrder(int userId);
		Task<ServiceResponse<List<OrderOverviewResponseDTO>>> GetOrders(int userId);
		Task<ServiceResponse<OrderDetailsResponseDTO>> GetOrderDetails(int userId, int orderId);
	}
}
