using Application.Dtos.CategoryDto;
using Application.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // ➤ CREATE
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(id);
        }

        // ➤ GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // ➤ GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        // ➤ UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCategoryDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // ➤ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
