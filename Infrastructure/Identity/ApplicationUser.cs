using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public Guid? TenantId { get; set; }   // NULL for SuperAdmin

        public bool IsGuest { get; set; }

    }
}
