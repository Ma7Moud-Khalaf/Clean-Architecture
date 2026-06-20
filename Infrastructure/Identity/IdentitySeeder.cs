using Application.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
   

    public static class IdentitySeeder
    {
        public static async Task SeedSuperAdminAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            // 1️⃣ Ensure Roles
            await EnsureRoleAsync(roleManager, AppRoles.SuperAdmin);
            await EnsureRoleAsync(roleManager, AppRoles.TenantAdmin);
            await EnsureRoleAsync(roleManager, AppRoles.Customer);

            // 2️⃣ SuperAdmin Data
            string adminEmail = "superadmin@system.com";
            string adminPassword = "SuperAdmin@123";

            var superAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (superAdmin == null)
            {
                superAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    PhoneNumber = "0000000000",
                    EmailConfirmed = true,
                    IsGuest = false,
                    TenantId = null // ✅ SuperAdmin = no tenant
                };

                var result = await userManager.CreateAsync(superAdmin, adminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create SuperAdmin: {string.Join(", ", result.Errors.Select(e => e.Description))}"
                    );
                }
            }

            // 3️⃣ Assign Role
            if (!await userManager.IsInRoleAsync(superAdmin, AppRoles.SuperAdmin))
            {
                await userManager.AddToRoleAsync(superAdmin, AppRoles.SuperAdmin);
            }
        }

        private static async Task EnsureRoleAsync(
            RoleManager<IdentityRole<Guid>> roleManager,
            string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }
    }
}
