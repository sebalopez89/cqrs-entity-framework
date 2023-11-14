using System.Threading;
using System.Threading.Tasks;
using CQRS.Application.Operation.Commands.Permission;
using CQRS.Application.Operation.Commands.Permission.Validators;
using CQRS.Persistence;
using CQRS.Persistence.Respositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using CQRS.UnitTests.Fixtures;
using Xunit;
using Xunit.Categories;
using CQRS.Application.Operation.Handlers.Permission;
using Nest;
using CQRS.Application.Helpers;

namespace CQRS.UnitTest.Application.Operation.Permission.Handlers;

[UnitTest]
public class CreatePermissionCommandHandlerTests : IClassFixture<EfSqliteFixture>
{
    private readonly CreatePermissionCommandValidator _validator = new();
    private readonly EfSqliteFixture _fixture;
    private readonly UnitOfWork _unitOfWork;

    public CreatePermissionCommandHandlerTests(EfSqliteFixture fixture)
    {
        _fixture = fixture;
        _unitOfWork = new UnitOfWork(
            _fixture.Context,
            new PermissionRepository(_fixture.Context, Substitute.For<ILogger<PermissionRepository>>()),
            new PermissionTypeRepository(_fixture.Context, Substitute.For<ILogger<PermissionTypeRepository>>()));
    }

    [Fact]
    public async Task Add_ValidCommand_ShouldReturnsSuccessResult()
    {
        // Arrange
        var command = new CreatePermissionCommand()
        {
            EmployeeForename = "testForename",
            EmployeeSurname = "testSurname",
            PermissionTypeId = 1
        };


        var handler = new CreatePermissionCommandHandler(
            Substitute.For<ILogger<CreatePermissionCommandHandler>>(),
            _validator,
            _unitOfWork,
            Substitute.For<IElasticClient>(),
            Substitute.For<IProducerMessageSender>());

        // Act
        var act = await handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().NotBeNull();
        act.IsSuccess.Should().BeTrue();
        act.SuccessMessage.Should().Be("Successfully registered!");
        act.Value.Should().NotBeNull();
        act.Value.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Add_InvalidCommand_ShouldReturnsFailResult()
    {
        // Arrange
        var command = new CreatePermissionCommand()
        {
            EmployeeSurname = "testSurname",
            PermissionTypeId = 1
        };

        var handler = new CreatePermissionCommandHandler(
            Substitute.For<ILogger<CreatePermissionCommandHandler>>(),
            _validator,
            _unitOfWork,
            Substitute.For<IElasticClient>(),
            Substitute.For<IProducerMessageSender>()
            );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().NotBeNull();
        act.IsSuccess.Should().BeFalse();
        act.ValidationErrors.Should().NotBeNullOrEmpty().And.OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task Add_NotFoundTypeCommand_ShouldReturnsNotFoundResult()
    {
        // Arrange
        var command = new CreatePermissionCommand()
        {
            EmployeeForename = "testForename",
            EmployeeSurname = "testSurname",
            PermissionTypeId = 3
        };

        var unitOfWork = new UnitOfWork(
            _fixture.Context,
            new PermissionRepository(_fixture.Context, Substitute.For<ILogger<PermissionRepository>>()),
            new PermissionTypeRepository(_fixture.Context, Substitute.For<ILogger<PermissionTypeRepository>>()));

        var handler = new CreatePermissionCommandHandler(
            Substitute.For<ILogger<CreatePermissionCommandHandler>>(),
            _validator,
            _unitOfWork,
            Substitute.For<IElasticClient>(),
            Substitute.For<IProducerMessageSender>());

        // Act
        var act = await handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().NotBeNull();
        act.IsSuccess.Should().BeFalse();
        act.Errors.Should()
            .NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.Contain(errorMessage => errorMessage == "Permission Type was not founded.");
    }
}