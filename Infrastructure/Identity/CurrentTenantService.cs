using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    using Application.Common;
    using Application.Interfaces.Common;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;

    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? TenantId
        {
            get
            {
                return Guid.Parse("8E6689CD-6DE3-497C-9920-E2E315D24599");
                //var user = _httpContextAccessor.HttpContext?.User;
                //if (user == null || !user.Identity!.IsAuthenticated)
                //    return null;

                //var tenantClaim = user.FindFirst("TenantId");
                //return tenantClaim != null ? Guid.Parse(tenantClaim.Value) : Guid.Parse("8E6689CD-6DE3-497C-9920-E2E315D24599");
            }
        }

        public bool IsSuperAdmin
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                return user?.IsInRole(AppRoles.SuperAdmin) ?? false;
            }
        }
    }

}
