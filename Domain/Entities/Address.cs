using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Address : AuditableEntity
    {
        public Guid UserId { get; set; }
        public string Area { get; set; } = default!;
        public string Block { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Building { get; set; } = default!;
        public string? Apartment { get; set; }

        public bool IsDefault { get; set; }
    }
}
