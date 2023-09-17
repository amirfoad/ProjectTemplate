using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(m =>
        {
            m.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}