using CQRS.Application.Contracts.Persistence;
using CQRS.Database;
using CQRS.Domain;
using CQRS.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Persistence.Respositories
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        private readonly PermissionsDbContext _dbContext;
        private readonly ILogger<PermissionRepository> _logger;

        public PermissionRepository(PermissionsDbContext dbContext, ILogger<PermissionRepository> logger) : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddPermissions(List<Permission> permissions)
        {
            await _dbContext.AddRangeAsync(permissions);
        }

        public async Task<List<Permission>> GetAllPermissions()
        {
            var permissions = await _dbContext.Permissions
               .Include(q => q.PermissionType)
               .ToListAsync();
            return permissions;
        }

        public async Task<Permission?> GetPermission(int id)
        {
            var permission = await _dbContext.Permissions
                .Include(q => q.PermissionType)
                .FirstOrDefaultAsync(q => q.Id == id);

            return permission;
        }
    }
}