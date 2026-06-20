using Application.Dtos.PaymentDto;
using Application.Services.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _service;

        public PaymentsController(PaymentService service)
        {
            _service = service;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(PaymentRequestDto dto)
        {
            var url = await _service.CheckoutAsync(dto);
            return Ok(new { url });
        }
    }
}
