using Application.Dtos.CategoryDto;
using Application.Interfaces.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTenantService _currentTenant;

        public CategoryService(
            IGenericRepository<Category> repository,
            IUnitOfWork unitOfWork,
            ICurrentTenantService currentTenant)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentTenant = currentTenant;
        }

        public async Task<Guid> CreateAsync(CreateCategoryDto dto)
        {
            var entity = new Category
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                TenantId = _currentTenant.TenantId!.Value
            };

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _repository.GetProjectedAsync(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<CategoryDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Category not found");

            return new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public async Task UpdateAsync(Guid id, UpdateCategoryDto dto)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Category not found");

            entity.Name = dto.Name;
            entity.Description = dto.Description;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Category not found");

            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
