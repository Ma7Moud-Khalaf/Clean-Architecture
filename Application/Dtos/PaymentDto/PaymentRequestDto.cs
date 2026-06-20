using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.PaymentDto
{
    public class PaymentRequestDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "KWD";
        public string OrderId { get; set; } = default!;
        public string CustomerEmail { get; set; } = default!;
        public string CustomerName { get; set; } = default!;
        public string CallbackUrl { get; set; } = default!;
    }
}
