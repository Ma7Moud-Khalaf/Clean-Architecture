using Application.Interfaces.Payment;
using Infrastructure.Payments.MyFatoorah;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Payment
{
    public class PaymentGatewayFactory : IPaymentGatewayFactory
    {
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _config;

        public PaymentGatewayFactory(IServiceProvider provider, IConfiguration config)
        {
            _provider = provider;
            _config = config;
        }

        public IPaymentGateway GetGateway()
        {
            var provider = _config["Payment:Provider"];

            return provider switch
            {
                "MyFatoorah" => _provider.GetRequiredService<MyFatoorahPaymentGateway>(),
                //"Tap" => _provider.GetRequiredService<TapPaymentGateway>(),
                "Fake" => _provider.GetRequiredService<FakePaymentGateway>(),
                _ => throw new Exception("Invalid payment provider")
            };
        }
    }
}
