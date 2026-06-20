using Application.Dtos.TenantDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Common
{
    public interface ITenantService
    {
        Task<Guid> CreateAsync(CreateTenantDto dto);
        Task<List<TenantDto>> GetAllAsync();
        Task<TenantDto> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, UpdateTenantDto dto);
        Task DeleteAsync(Guid id);
    }
}
