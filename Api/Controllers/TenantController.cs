using Application.Dtos.TenantDto;
using Application.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    { 
            private readonly ITenantService _tenantService;

            public TenantController(ITenantService tenantService)
            {
                _tenantService = tenantService;
            }

            // ➤ CREATE
            [HttpPost]
            public async Task<IActionResult> Create(CreateTenantDto dto)
            {
                var id = await _tenantService.CreateAsync(dto);
                return Ok(id);
            }

            // ➤ GET ALL
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var result = await _tenantService.GetAllAsync();
                return Ok(result);
            }

            // ➤ GET BY ID
            [HttpGet("{id:guid}")]
            public async Task<IActionResult> GetById(Guid id)
            {
                var result = await _tenantService.GetByIdAsync(id);
                return Ok(result);
            }

            // ➤ UPDATE
            [HttpPut("{id:guid}")]
            public async Task<IActionResult> Update(Guid id, UpdateTenantDto dto)
            {
                await _tenantService.UpdateAsync(id, dto);
                return NoContent();
            }

            // ➤ DELETE (soft delete if implemented in entity)
            [HttpDelete("{id:guid}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                await _tenantService.DeleteAsync(id);
                return NoContent();
            }
        }
    }


