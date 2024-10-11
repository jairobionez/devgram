using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Devgram.Data.Infra
{
    public class DevgramDbContext : IdentityDbContext<IdentityUser>
    {

        protected DevgramDbContext()
        {

        }

        public DevgramDbContext(DbContextOptions<DevgramDbContext> options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DevgramDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}