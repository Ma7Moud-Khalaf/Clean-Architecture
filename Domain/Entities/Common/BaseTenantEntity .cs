using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public class BaseTenantEntity : AuditableEntity
    {
        public Guid TenantId { get; set; }
    }
}
