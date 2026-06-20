using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Common
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[]? includes);
        IQueryable<T> Query();
        Task<List<T>> GetAllAsync();
        Task<PaginatedResult<T>> GetPagedAsync(int pageNumber, int pageSize,
                                                        Expression<Func<T, bool>>? filter = null,
                                                     params Expression<Func<T, object>>[]? includes);
        Task<List<TResult>> GetProjectedAsync<TResult>(
    Expression<Func<T, TResult>> selector);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<bool> ExistsAsync(Guid id);
    }

}
