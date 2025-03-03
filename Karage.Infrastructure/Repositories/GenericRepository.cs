using Karage.Domain.Entities;
using Karage.Domain.Interfaces;
using Karage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Karage.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly KarageDBContext context;

        public GenericRepository(KarageDBContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().OrderByDescending(e => e.Id).Where(e => !e.IsDeleted).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public async Task AddAsync(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            try
            { 
                entity.IsDeleted = false;
            }
            catch { }
            await context.Set<T>().AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            var existingEntity = await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (existingEntity != null)
            {
                entity.CreatedDate = existingEntity.CreatedDate;
                entity.ModifiedDate = DateTime.UtcNow;
                context.Set<T>().Update(entity);
            }
            else
            {
                await AddAsync(entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                context.Set<T>().Update(entity);
            }
        }
    }
}
