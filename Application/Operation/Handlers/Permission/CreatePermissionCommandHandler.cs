using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using CQRS.Application.Contracts.Persistence;
using CQRS.Application.Operation.Commands.Permission;
using CQRS.Application.Operation.Responses;
using FluentValidation;
using MediatR;

namespace CQRS.Application.Operation.Handlers.Permission
{

    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Result<BaseCommandResponse>>
    {
        private readonly IValidator<CreatePermissionCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePermissionCommandHandler(
            IValidator<CreatePermissionCommand> validator,
            IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<BaseCommandResponse>> Handle(
            CreatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<BaseCommandResponse>.Invalid(validationResult.AsErrors());
            }
            var permisssionType = await _unitOfWork.PermissionTypeRepository.Get(request.PermissionTypeId);
            if (permisssionType == null)
            {
                return Result<BaseCommandResponse>.NotFound("Permission Type was not founded.");
            }

            var permisssion = new Domain.Permission()
            {
                EmployeeForename = request.EmployeeForename,
                EmployeeSurname = request.EmployeeSurname,
                PermissionDate = System.DateTime.Now,
                PermissionType = permisssionType,
            };

            await _unitOfWork.PermissionRepository.Add(permisssion);

            await _unitOfWork.Save();

            return Result<BaseCommandResponse>.Success(
                new BaseCommandResponse(permisssion.Id), "Successfully registered!");
        }
    }

}