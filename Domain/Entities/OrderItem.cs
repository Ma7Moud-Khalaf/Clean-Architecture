using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem: AuditableEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;

        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
