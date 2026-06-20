using Application.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GenericService<T> : IGenericService<T>
     where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public GenericService(
            IGenericRepository<T> repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<T?> GetByIdAsync(Guid id)
            => await _repository.GetByIdAsync(id);

        public async Task<T> CreateAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Entity not found");

            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
