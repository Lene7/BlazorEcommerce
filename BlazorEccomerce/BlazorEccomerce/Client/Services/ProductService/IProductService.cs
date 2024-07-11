﻿namespace BlazorEccomerce.Client.Services.ProductService
{
	public interface IProductService
	{
		event Action ProductsChanged;

		List<Product> Products { get; set; }
		string Messsage { get; set; }

		Task GetProducts(string? categoryUrl = null);
		Task<ServiceResponse<Product>> GetProduct(int productId);
		Task SearchProducts(string searchText);
		Task<List<string>> GetProductSearchSuggestions(string searchText);
	}
}
