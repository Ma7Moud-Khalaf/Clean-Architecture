using Domain.Entities.Common;
using Domain.Entities.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class Category : BaseTenantEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

    }

}
