using Application.Dtos.ProductDto;
using Application.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductDto request)
        {
            if (request.Images == null || request.Images.Count == 0)
                return BadRequest("No images uploaded");

            var command = new CreateProductDto
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId,
                Images = request.Images
            };

            var productId = await _productService.CreateAsync(command);
            return Ok(productId);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto request)
        {
            await _productService.UpdateAsync(request);
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdWithImagesAsync(id);
            return Ok(product);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _productService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}
