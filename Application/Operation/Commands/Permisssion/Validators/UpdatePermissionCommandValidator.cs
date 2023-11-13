using FluentValidation;

namespace CQRS.Application.Operation.Commands.Permission.Validators
{
    public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionCommandValidator()
        {
            RuleFor(command => command.EmployeeForename)
                .MaximumLength(100);

            RuleFor(command => command.EmployeeSurname)
                .MaximumLength(100);

            RuleFor(command => command.PermissionTypeId);
        }
    }
}
