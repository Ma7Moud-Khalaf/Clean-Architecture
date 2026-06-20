using Application.Dtos.PaymentDto;
using Application.Interfaces.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Payments.MyFatoorah
{
    public class MyFatoorahPaymentGateway : IPaymentGateway
    {
        private readonly HttpClient _http;

        public MyFatoorahPaymentGateway(HttpClient http)
        {
            _http = http;
        }

        public async Task<PaymentResponseDto> CreatePaymentAsync(PaymentRequestDto request)
        {
            var payload = new
            {
                InvoiceValue = request.Amount,
                CurrencyIso = request.Currency,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CallBackUrl = request.CallbackUrl
            };

            var response = await _http.PostAsJsonAsync("/v2/ExecutePayment", payload);
            var result = await response.Content.ReadFromJsonAsync<dynamic>();

            return new PaymentResponseDto
            {
                IsSuccess = true,
                PaymentUrl = result?.Data?.InvoiceURL,
                TransactionId = result?.Data?.InvoiceId
            };
        }

        public async Task<bool> VerifyPaymentAsync(string transactionId)
        {
            var res = await _http.GetAsync($"/v2/GetPaymentStatus?invoiceId={transactionId}");
            return res.IsSuccessStatusCode;
        }
    }
}
