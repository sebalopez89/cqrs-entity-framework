using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.Database
{
    public static class DatabaseServicesRegistration
    {
        public static IServiceCollection ConfigureDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PermissionsDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("PermissionsConnectionString")));

            return services;
        }
    }
}
