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

    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, Result<BaseCommandResponse>>
    {
        private readonly IValidator<UpdatePermissionCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePermissionCommandHandler(
            IValidator<UpdatePermissionCommand> validator,
            IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<BaseCommandResponse>> Handle(
            UpdatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<BaseCommandResponse>.Invalid(validationResult.AsErrors());
            }
            var permisssion = await _unitOfWork.PermissionRepository.Get(request.Id);
            if (permisssion == null)
            {
                return Result<BaseCommandResponse>.NotFound("Permission was not founded.");
            }
            if (request.PermissionTypeId != null)
            {
                var permisssionType = await _unitOfWork.PermissionTypeRepository.Get(request.PermissionTypeId.Value);
                if (permisssionType == null)
                {
                    return Result<BaseCommandResponse>.NotFound("Permission Type was not founded.");
                }
                permisssion.PermissionType = permisssionType;
            }
            if (request.EmployeeForename != null)
            {
                permisssion.EmployeeForename = request.EmployeeForename;
            }
            if (request.EmployeeSurname != null)
            {
                permisssion.EmployeeSurname = request.EmployeeSurname;
            }

            _unitOfWork.PermissionRepository.Update(permisssion);

            await _unitOfWork.Save();

            return Result<BaseCommandResponse>.Success(
                new BaseCommandResponse(permisssion.Id), "Successfully updated!");
        }
    }

}