using MediatR;
using CQRS.Application.Operation.Responses;
using Ardalis.Result;

namespace CQRS.Application.Operation.Commands.Permission
{
    public class UpdatePermissionCommand : IRequest<Result<BaseCommandResponse>>
    {
        public int Id { get; set; }
        public string? EmployeeForename { get; set; }
        public string? EmployeeSurname { get; set; }
        public int? PermissionTypeId { get; set; }
    }
}