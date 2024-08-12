﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEccomerce.Shared
{
	public class CartItem
	{
		public int Id { get; set; } 

		public int CartId { get; set; } 
		public Cart Cart { get; set; }

        public int ProductVariantId { get; set; }
		public ProductVariant ProductVariant { get; set; }

        public int ProductId { get; set; }
		public int Quantity { get; set; } = 1;


		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }
	}
}
