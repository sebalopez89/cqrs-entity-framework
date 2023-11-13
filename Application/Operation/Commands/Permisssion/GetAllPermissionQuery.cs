using MediatR;
using Ardalis.Result;
using CQRS.Application.Operation.Responses.Permission;

namespace CQRS.Application.Operation.Commands.Permission
{
    public class GetAllPermissionsQuery : IRequest<Result<GetAllPermissionsQueryResponse>>
    {
    }
}