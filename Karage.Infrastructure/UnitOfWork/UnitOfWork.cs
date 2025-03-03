using Karage.Domain.Interfaces;
using Karage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore; 

namespace Karage.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private KarageDBContext _DbContext;
        public DbContext context => _DbContext;
        public UnitOfWork(KarageDBContext context)
        {
            this._DbContext = context;
        }

        public int SaveChanges()
        {
            return this._DbContext.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await this._DbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            this._DbContext.Dispose();
        }
        public async Task DisposeAsync()
        {
            await this._DbContext.DisposeAsync();
        }
    }
}
