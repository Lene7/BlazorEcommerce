using System;
using System.Collections.Generic;
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

		public int ProductId { get; set; } 
		public Product Product { get; set; } 

		public int ProductTypeId { get; set; } 
		public ProductType ProductType { get; set; } 

		public int Quantity { get; set; } = 1; 
		public decimal Price { get; set; } 

		public DateTime DateAdded { get; set; } = DateTime.Now; 
	}
}
