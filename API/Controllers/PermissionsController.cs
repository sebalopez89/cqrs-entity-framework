using CQRS.Application.Operation.Commands.Permission;
using CQRS.Application.Operation.Dtos;
using CQRS.Application.Operation.Responses;
using CQRS.Application.Operation.Responses.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CQRS.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly ILogger<PermissionsController> _logger;
        private readonly IMediator _mediator;

        public PermissionsController(
            ILogger<PermissionsController> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<BaseCommandResponse>> CreatePermission([FromBody] CreatePermissionCommand command)
        {
            _logger.LogInformation("Create method: ", command);
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }
            return BadRequest(response.Errors);
        }

        [HttpPatch]
        [Route("{Id}")]
        public async Task<ActionResult<BaseCommandResponse>> UpdatePermission([FromRoute] int Id, [FromBody] UpdatePermissionCommandDTO command)
        {
            _logger.LogInformation("update method: ", command);
            var updateCommand = new UpdatePermissionCommand(){
                EmployeeSurname = command.EmployeeSurname,
                EmployeeForename = command.EmployeeForename,
                Id = Id,
                PermissionTypeId = command.PermissionTypeId,
            };
            var response = await _mediator.Send(updateCommand);
            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }
            return BadRequest(response.Errors);
        }

        [HttpGet]
        public async Task<ActionResult<GetAllPermissionsQueryResponse>> GetPermissions()
        {
            _logger.LogInformation("get method");
            var query = new GetAllPermissionsQuery();
            var response = await _mediator.Send(query);
            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }
            return BadRequest(response.Errors);
        }
    }
}