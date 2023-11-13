using CQRS.Application.Contracts.Persistence;
using CQRS.Database;
using System;
using System.Threading.Tasks;

namespace CQRS.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly PermissionsDbContext _context;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;


        public UnitOfWork(
            PermissionsDbContext context,
            IPermissionRepository permissionRepository,
            IPermissionTypeRepository permissionTypeRepository
            )
        {
            _context = context;
            _permissionRepository = permissionRepository;
            _permissionTypeRepository = permissionTypeRepository;
        }

        public IPermissionTypeRepository PermissionTypeRepository => _permissionTypeRepository;

        public IPermissionRepository PermissionRepository => _permissionRepository;

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save() 
        {
            await _context.SaveChangesAsync();
        }
    }
}
