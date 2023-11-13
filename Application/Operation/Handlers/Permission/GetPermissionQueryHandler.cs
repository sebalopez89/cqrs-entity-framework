using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using CQRS.Application.Contracts.Persistence;
using CQRS.Application.Helpers;
using CQRS.Application.Operation.Commands.Permission;
using CQRS.Application.Operation.Responses.Permission;
using MediatR;
using Nest;

namespace CQRS.Application.Operation.Handlers.Permission
{

    public class GetPermissionQueryHandler : IRequestHandler<GetAllPermissionsQuery, Result<GetAllPermissionsQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticClient _elasticClient;

        public GetPermissionQueryHandler(
            IUnitOfWork unitOfWork,
            IElasticClient elasticClient)
        {
            _unitOfWork = unitOfWork;
            _elasticClient = elasticClient;
        }

        public async Task<Result<GetAllPermissionsQueryResponse>> Handle(
            GetAllPermissionsQuery request,
            CancellationToken cancellationToken)
        {
            var permisssions = await _unitOfWork.PermissionRepository.GetAllPermissions();

            if (permisssions.Any())
            {
                var response = await _elasticClient.IndexDocumentAsync(permisssions.First());
            }

            ProducerMessageSender.SendMessage(new ProducerMessage("Get"));

            return Result<GetAllPermissionsQueryResponse>.Success(
                new GetAllPermissionsQueryResponse() { 
                    Items = permisssions.Select(item => new GetAllPermissionsQueryResponseItem(
                        item.Id,
                        item.EmployeeForename,
                        item.EmployeeSurname,
                        item.PermissionType.Id,
                        item.PermissionType.Description)).ToList()
                }, "Successfully retrieved!");
        }
    }

}