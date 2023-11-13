using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CQRS.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services
                .AddValidatorsFromAssembly(assembly)
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            return services;
        }
    }
}
