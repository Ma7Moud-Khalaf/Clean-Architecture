using Application.Interfaces.Common;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace Application
    {
        public static class DependencyInjection
        {
            public static IServiceCollection AddApplication(
                this IServiceCollection services)
            {
                // ✅ Generic services
                services.AddScoped(typeof(IGenericService<>),
                                   typeof(GenericService<>));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ICategoryService, CategoryService>();



            // ✅ other application services later
            // services.AddScoped<IOrderService, OrderService>();
            // services.AddScoped<ITenantService, TenantService>();

            return services;
            }
        }
   
}
