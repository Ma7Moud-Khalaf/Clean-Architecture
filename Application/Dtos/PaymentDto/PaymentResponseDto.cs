using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.PaymentDto
{
    public class PaymentResponseDto
    {
        public bool IsSuccess { get; set; }
        public string PaymentUrl { get; set; } = default!;
        public string? TransactionId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
