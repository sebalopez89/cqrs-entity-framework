using CQRS.Application.Helpers;
using CQRS.Domain;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Reflection;

namespace CQRS.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services
                .AddValidatorsFromAssembly(assembly)
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            var settings = new ConnectionSettings(new Uri(configuration["ElasticConfiguration:Uri"]))
            .DefaultMappingFor<Permission>(i => i
                .IndexName(configuration["ElasticConfiguration:Index"])
                .IdProperty(p => p.Id)
            )
            .DefaultIndex(configuration["ElasticConfiguration:Index"]);

            var client = new ElasticClient(settings);
                
            services.AddScoped<IElasticClient>(_ => client);
            services.AddScoped<IProducerMessageSender, ProducerMessageSender>();

            return services;
        }
    }
}
