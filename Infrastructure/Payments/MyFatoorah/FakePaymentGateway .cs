using Application.Dtos.PaymentDto;
using Application.Interfaces.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Payments.MyFatoorah
{
    public class FakePaymentGateway : IPaymentGateway
    {
        public Task<PaymentResponseDto> CreatePaymentAsync(PaymentRequestDto request)
        {
            return Task.FromResult(new PaymentResponseDto
            {
                IsSuccess = true,
                PaymentUrl = "https://fake-payment.com/checkout/" + Guid.NewGuid(),
                TransactionId = Guid.NewGuid().ToString()
            });
        }

        public Task<bool> VerifyPaymentAsync(string transactionId)
        {
            return Task.FromResult(true);
        }
    }
}
