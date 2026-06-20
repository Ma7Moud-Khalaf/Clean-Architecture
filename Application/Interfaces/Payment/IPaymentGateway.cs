using Application.Dtos.PaymentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Payment
{
    public interface IPaymentGateway
    {
        Task<PaymentResponseDto> CreatePaymentAsync(PaymentRequestDto request);
        Task<bool> VerifyPaymentAsync(string transactionId);
    }
}
