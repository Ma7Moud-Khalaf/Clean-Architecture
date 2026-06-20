using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class Product : BaseTenantEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public ICollection<ProductImage> Images { get; set; } = [];

    }

}
