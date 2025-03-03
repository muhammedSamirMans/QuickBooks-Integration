using Microsoft.EntityFrameworkCore; 

namespace Karage.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbContext context { get; }
    }
}
