using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEccomerce.Shared
{
	public class CartDetailDTO
	{
		public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
		public List<CartProductResponseDTO> Products { get; set; } = new List<CartProductResponseDTO>();
	}
}
