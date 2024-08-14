using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEccomerce.Shared
{
	public  class ProductVariantDTO
	{
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal OriginalPrice { get; set; }
    }
}
