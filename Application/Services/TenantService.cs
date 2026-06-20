using Application.Dtos.TenantDto;
using Application.Interfaces.Common;
using Domain.Entities.Tenants;

namespace Application.Services
{
    public class TenantService : ITenantService
    {
        private readonly IGenericRepository<Tenant> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTenantService _currentTenant;

        public TenantService(
            IGenericRepository<Tenant> repository,
            IUnitOfWork unitOfWork,
            ICurrentTenantService currentTenant)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentTenant = currentTenant;
        }

        public async Task<Guid> CreateAsync(CreateTenantDto dto)
        {
            var entity = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                LogoUrl = dto.LogoUrl,
                Description = dto.Description,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                IsActive = dto.IsActive
            };

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<List<TenantDto>> GetAllAsync()
        {
            return await _repository.GetProjectedAsync(t => new TenantDto
            {
                Id = t.Id,
                Name = t.Name,
                LogoUrl = t.LogoUrl,
                Description = t.Description,
                PhoneNumber = t.PhoneNumber,
                Email = t.Email,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt
            });
        }

        public async Task<TenantDto> GetByIdAsync(Guid id)
        {
            var t = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Tenant not found");

            return new TenantDto
            {
                Id = t.Id,
                Name = t.Name,
                LogoUrl = t.LogoUrl,
                Description = t.Description,
                PhoneNumber = t.PhoneNumber,
                Email = t.Email,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt
            };
        }

        public async Task UpdateAsync(Guid id, UpdateTenantDto dto)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Tenant not found");

            entity.Name = dto.Name;
            entity.LogoUrl = dto.LogoUrl;
            entity.Description = dto.Description;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Email = dto.Email;
            entity.IsActive = dto.IsActive;

            _repository.Update(entity); // ⚠️ important if your repo requires it
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Tenant not found");

            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}