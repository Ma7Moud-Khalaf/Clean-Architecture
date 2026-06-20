using Application.Interfaces.Common;
using Application.Interfaces.Payment;
using Application.Services.Payment;
using Infrastructure.DbContext;
using Infrastructure.Identity;
using Infrastructure.Payments.MyFatoorah;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
        {
            // DbContext
           // services.AddDbContext<AppDbContext>(...);

            // Identity
            //services.AddIdentityCore<ApplicationUser, IdentityRole<Guid>>()
            //        .AddEntityFrameworkStores<AppDbContext>();

            // Repositories
            services.AddScoped(typeof(IGenericRepository<>),
                               typeof(GenericRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ICurrentTenantService, CurrentTenantService>();

            services.AddHttpClient<MyFatoorahPaymentGateway>();
            services.AddScoped<FakePaymentGateway>();           // services.AddHttpClient<TapPaymentGateway>();

            services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();
            services.AddScoped<PaymentService>();


            return services;
        }
    }
}
