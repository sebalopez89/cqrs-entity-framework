using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using CQRS.Application.Contracts.Persistence;
using CQRS.Application.Operation.Commands.Permission;
using CQRS.Application.Operation.Responses.Permission;
using MediatR;

namespace CQRS.Application.Operation.Handlers.Permission
{

    public class GetPermissionQueryHandler : IRequestHandler<GetAllPermissionsQuery, Result<GetAllPermissionsQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPermissionQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetAllPermissionsQueryResponse>> Handle(
            GetAllPermissionsQuery request,
            CancellationToken cancellationToken)
        {
            var permisssionType = await _unitOfWork.PermissionRepository.GetAllPermissions();
            return Result<GetAllPermissionsQueryResponse>.Success(
                new GetAllPermissionsQueryResponse() { 
                    Items = permisssionType.Select(item => new GetAllPermissionsQueryResponseItem(
                        item.Id,
                        item.EmployeeForename,
                        item.EmployeeSurname,
                        item.PermissionType.Id,
                        item.PermissionType.Description)).ToList()
                }, "Successfully registered!");
        }
    }

}