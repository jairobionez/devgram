using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Devgram.Infra
{
    public class DbContext : IdentityDbContext<IdentityUser>
    {

        protected DbContext()
        {

        }

        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}