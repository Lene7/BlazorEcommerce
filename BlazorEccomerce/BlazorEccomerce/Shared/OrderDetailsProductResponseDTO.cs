using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEccomerce.Shared
{
	public class OrderDetailsProductResponseDTO
	{
        public int ProductVariantId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public string ProductImageURL { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
