using Karage.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Karage.Infrastructure.Data
{
    public class KarageDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<QBAuth> QBAuths { get; set; }

        public KarageDBContext(DbContextOptions<KarageDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
