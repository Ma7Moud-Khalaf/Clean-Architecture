using Domain.Entities.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : BaseTenantEntity
    {
        public Guid UserId { get; set; } 
        public Guid AddressId { get; set; }
        public Address Address { get; set; } = default!;

        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
