using System.ComponentModel.DataAnnotations;
using MediatR;
using CQRS.Application.Operation.Responses;
using Ardalis.Result;

namespace CQRS.Application.Operation.Commands.Permission
{
    public class CreatePermissionCommand : IRequest<Result<BaseCommandResponse>>
    {
        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string EmployeeForename { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string EmployeeSurname { get; set; }

        [Required]
        public int PermissionTypeId { get; set; }
    }
}