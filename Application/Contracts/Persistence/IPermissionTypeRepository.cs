using CQRS.Domain;
using System.Threading.Tasks;

namespace CQRS.Application.Contracts.Persistence
{
    public interface IPermissionTypeRepository : IGenericRepository<PermissionType>
    {
        Task<PermissionType?> GetPermissionType(int id);
    }
}
