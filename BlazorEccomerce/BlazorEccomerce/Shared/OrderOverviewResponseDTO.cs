using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEccomerce.Shared
{
    public class OrderOverviewResponseDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductSummary { get; set; }
        public string ProductImageUrl { get; set; }
    }
}
