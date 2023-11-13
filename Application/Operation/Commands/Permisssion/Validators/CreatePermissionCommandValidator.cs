using FluentValidation;

namespace CQRS.Application.Operation.Commands.Permission.Validators
{
    public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionCommandValidator()
        {
            RuleFor(command => command.EmployeeForename)
                .NotNull()
                .MaximumLength(100);

            RuleFor(command => command.EmployeeSurname)
                .NotNull()
                .MaximumLength(100);

            RuleFor(command => command.PermissionTypeId)
                .NotNull();
        }
    }
}
