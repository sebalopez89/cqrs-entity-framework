using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ardalis.Result;
using CQRS.Application.Operation.Commands.Permission;
using CQRS.Application.Operation.Responses;
using CQRS.Database;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Categories;

namespace CQRS.IntegrationTest.Controllers;

[IntegrationTest]
public class CustomersControllerTests : IAsyncLifetime
{
    private const string ConnectionString = "Data Source=:memory:";
    private const string Endpoint = "permissions";
    private readonly SqliteConnection _permissionDbContextSqlite = new(ConnectionString);

    #region POST: /api/permission/

    [Fact]
    public async Task Should_ReturnsHttpStatus200Ok_When_Post_ValidRequest()
    {
        // Arrange
        await using var webApplicationFactory = InitializeWebAppFactory();
        using var httpClient = webApplicationFactory.CreateClient(CreateClientOptions());

        var command = new CreatePermissionCommand()
        {
            EmployeeForename = "ForenameTest",
            EmployeeSurname = "SurnameTest",
            PermissionTypeId = 1,
        };

        var commandAsJsonString = JsonSerializer.Serialize(command);

        // Act
        using var jsonContent = new StringContent(commandAsJsonString, Encoding.UTF8, MediaTypeNames.Application.Json);
        using var act = await httpClient.PostAsync(Endpoint, jsonContent);

        // Assert (HTTP)
        act.Should().NotBeNull();
        act.IsSuccessStatusCode.Should().BeTrue();
        act.StatusCode.Should().Be(HttpStatusCode.OK);

        // Assert (HTTP Content Response)
        var response = JsonSerializer.Deserialize<Result<BaseCommandResponse>>(await act.Content.ReadAsStringAsync());
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_ReturnsHttpStatus400BadRequest_When_Post_InvalidRequest()
    {
        // Arrange
        await using var webApplicationFactory = InitializeWebAppFactory();
        using var httpClient = webApplicationFactory.CreateClient(CreateClientOptions());

        var command = new CreatePermissionCommand()
        {
            EmployeeForename = "",
            EmployeeSurname = "SurnameTest",
            PermissionTypeId = 1,
        };

        var commandAsJsonString = JsonSerializer.Serialize(command);

        // Act
        using var jsonContent = new StringContent(commandAsJsonString, Encoding.UTF8, MediaTypeNames.Application.Json);
        using var act = await httpClient.PostAsync(Endpoint, jsonContent);

        // Assert (HTTP)
        act.Should().NotBeNull();
        act.IsSuccessStatusCode.Should().BeFalse();
        act.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnsHttpStatus400NotFound_When_Post_NotExistPermissionType()
    {
        // Arrange
        await using var webApplicationFactory = InitializeWebAppFactory();
        using var httpClient = webApplicationFactory.CreateClient(CreateClientOptions());

        var command = new CreatePermissionCommand()
        {
            EmployeeForename = "ForenameTest",
            EmployeeSurname = "SurnameTest",
            PermissionTypeId = 3,
        };

        var commandAsJsonString = JsonSerializer.Serialize(command);

        // Act
        using var jsonContent = new StringContent(commandAsJsonString, Encoding.UTF8, MediaTypeNames.Application.Json);
        using var act = await httpClient.PostAsync(Endpoint, jsonContent);

        // Assert (HTTP)
        act.Should().NotBeNull();
        act.IsSuccessStatusCode.Should().BeFalse();
        act.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region IAsyncLifetime

    public async Task InitializeAsync()
    {
        await _permissionDbContextSqlite.OpenAsync();
    }

    public async Task DisposeAsync()
    {
        await _permissionDbContextSqlite.DisposeAsync();
    }

    #endregion

    #region Helpers

    public WebApplicationFactory<Program> InitializeWebAppFactory(
        Action<IServiceCollection> configureServices = null,
        Action<IServiceScope> configureServiceScope = null)
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(hostBuilder =>
            {
                hostBuilder.UseSetting("ConnectionStrings:PermissionsConnectionString", "InMemory");

                hostBuilder.UseSetting("CacheOptions:AbsoluteExpirationInHours", "1");
                hostBuilder.UseSetting("CacheOptions:SlidingExpirationInSeconds", "30");

                hostBuilder.UseEnvironment(Environments.Development);

                hostBuilder.ConfigureLogging(logging => logging.ClearProviders());

                hostBuilder.ConfigureServices(services =>
                {
                    services.RemoveAll<PermissionsDbContext>();
                    services.RemoveAll<DbContextOptions<PermissionsDbContext>>();

                    services.AddDbContext<PermissionsDbContext>(
                        options => options.UseSqlite(_permissionDbContextSqlite));

                    configureServices?.Invoke(services);

                    using var serviceProvider = services.BuildServiceProvider(true);
                    using var serviceScope = serviceProvider.CreateScope();

                    var permissionsDbContext = serviceScope.ServiceProvider.GetRequiredService<PermissionsDbContext>();
                    permissionsDbContext.Database.EnsureCreated();

                    configureServiceScope?.Invoke(serviceScope);

                    permissionsDbContext.Dispose();
                });
            });
    }

    private static WebApplicationFactoryClientOptions CreateClientOptions() => new() { AllowAutoRedirect = false };

    #endregion
}