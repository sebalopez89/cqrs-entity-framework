using CQRS.Domain;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Database
{
    public class PermissionsDbContext : DbContext
    {
        public PermissionsDbContext(DbContextOptions<PermissionsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PermissionsDbContext).Assembly);
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
    }
}
