using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Tenants
{
    public class Tenant : AuditableEntity
    {
     
        public string Name { get; set; } = default!;
 
        public string? LogoUrl { get; set; }

        public string? Description { get; set; }

        public string PhoneNumber { get; set; } = default!;
        public string? Email { get; set; }

        public bool IsActive { get; set; }
    }
}
