using Application.Dtos.PaymentDto;
using Application.Interfaces.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Payment
{
    public class PaymentService
    {
        private readonly IPaymentGatewayFactory _factory;

        public PaymentService(IPaymentGatewayFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> CheckoutAsync(PaymentRequestDto request)
        {
            var gateway = _factory.GetGateway();

            var result = await gateway.CreatePaymentAsync(request);

            if (!result.IsSuccess)
                throw new Exception(result.ErrorMessage);

            return result.PaymentUrl;
        }
    }
}
