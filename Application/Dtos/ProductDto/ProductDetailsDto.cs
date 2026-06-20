using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ProductDto
{
    public class ProductDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }

        public List<ProductImageDto> Images { get; set; } = new();
    }
    public class ProductImageDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public bool IsMain { get; set; }
    }
}
