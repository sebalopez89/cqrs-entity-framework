using CQRS.Application.Contracts.Persistence;
using CQRS.Database;
using CQRS.Domain;
using CQRS.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CQRS.Persistence.Respositories
{
    public class PermissionTypeRepository : GenericRepository<PermissionType>, IPermissionTypeRepository
    {
        private readonly PermissionsDbContext _dbContext;
        private readonly ILogger<PermissionTypeRepository> _logger;

        public PermissionTypeRepository(PermissionsDbContext dbContext, ILogger<PermissionTypeRepository> logger) : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PermissionType?> GetPermissionType(int id)
        {
            var permissionType = await _dbContext.PermissionTypes
                .FirstOrDefaultAsync(q => q.Id == id);

            return permissionType;
        }
    }
}