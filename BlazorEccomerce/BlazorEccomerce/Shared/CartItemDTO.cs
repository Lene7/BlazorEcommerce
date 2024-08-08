using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEccomerce.Shared
{
	public class CartItemDTO
	{
		public int ProductVariantId { get; set; }
		public int ProductTypeId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
