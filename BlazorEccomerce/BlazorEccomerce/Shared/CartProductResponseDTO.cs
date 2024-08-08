using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEccomerce.Shared
{
	public class CartProductResponseDTO
	{
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
		public int Quantity { get; set; }
		public int ProductVariantId { get; set; }

		public CartProductResponseDTO(int productId, string title, int productTypeId, string productType, string imageUrl, decimal price, int quantity, int productVariantId)
		{
			ProductId = productId;
			Title = title;
			ProductTypeId = productTypeId;
			ProductType = productType;
			ImageUrl = imageUrl;
			Price = price;
			Quantity = quantity;
			ProductVariantId = productVariantId;
		}

		
		public CartProductResponseDTO() { }
	}
}
