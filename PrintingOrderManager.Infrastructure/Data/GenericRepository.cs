// PrintingOrderManager.Infrastructure.Repositories/GenericRepository.cs
using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.Interfaces;
using PrintingOrderManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintingOrderManager.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return _context.Set<T>(); // ← ВАЖНО: возвращает IQueryable для ProjectTo
        }

        public virtual async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync(); // ✅ ОБЯЗАТЕЛЬНО
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(); // ✅ ОБЯЗАТЕЛЬНО
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync(); // ✅ ОБЯЗАТЕЛЬНО
            }
        }
    }
}