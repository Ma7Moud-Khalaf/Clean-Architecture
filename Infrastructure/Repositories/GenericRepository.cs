using Application.Common;
using Application.Interfaces.Common;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{ 
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id,params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes is { Length: > 0 })
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }
        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable().AsNoTracking();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        public async Task<PaginatedResult<T>> GetPagedAsync(int pageNumber,int pageSize,
                                                        Expression<Func<T, bool>>? filter = null,
                                                        params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _context.Set<T>();

            // filter
            if (filter != null)
                query = query.Where(filter);

            // includes
            if (includes is { Length: > 0 })
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<List<TResult>> GetProjectedAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            return await _dbSet
                .AsNoTracking()
                .Select(selector)
                .ToListAsync();
        }
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }
}
