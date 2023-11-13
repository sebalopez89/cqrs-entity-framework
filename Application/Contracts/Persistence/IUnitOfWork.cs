using System;
using System.Threading.Tasks;

namespace CQRS.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IPermissionRepository PermissionRepository { get; }
        IPermissionTypeRepository PermissionTypeRepository { get; }
        Task Save();
    }
}
