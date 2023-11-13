using CQRS.Application.Contracts.Persistence;
using CQRS.Persistence.Respositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.Persistence
{
    public static class PersistanceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistanceServices(this IServiceCollection services)
        {
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
