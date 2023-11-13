using CQRS.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Application.Contracts.Persistence
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<Permission?> GetPermission(int id);
        Task<List<Permission>> GetAllPermissions();
        Task AddPermissions(List<Permission> permissions);
    }
}
