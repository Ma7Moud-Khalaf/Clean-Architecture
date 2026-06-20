using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class ProductImage: AuditableEntity
    {
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; } = default!;
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
        public string? AltText { get; set; }
    }

}
