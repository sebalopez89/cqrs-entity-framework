using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using CQRS.Application.Contracts.Persistence;
using CQRS.Application.Helpers;
using CQRS.Application.Operation.Commands.Permission;
using CQRS.Application.Operation.Responses;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Nest;

namespace CQRS.Application.Operation.Handlers.Permission
{

    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Result<BaseCommandResponse>>
    {
        private readonly ILogger<CreatePermissionCommandHandler> _logger;
        private readonly IValidator<CreatePermissionCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticClient _elasticClient;

        public CreatePermissionCommandHandler(
            ILogger<CreatePermissionCommandHandler> logger,
            IValidator<CreatePermissionCommand> validator,
            IUnitOfWork unitOfWork,
            IElasticClient elasticClient)
        {
            _logger = logger;
            _validator = validator;
            _unitOfWork = unitOfWork;
            _elasticClient = elasticClient;
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

            var response = await _elasticClient.IndexDocumentAsync(permisssion);

            ProducerMessageSender.SendMessage(new ProducerMessage("Create"));

            return Result<BaseCommandResponse>.Success(
                new BaseCommandResponse(permisssion.Id), "Successfully registered!");
        }
    }

}