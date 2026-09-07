using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.Interfaces;
using Restaurant.Infrastructure.Data;
using Restaurant.Infrastructure.Repositories;

namespace Restaurant.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IMealRepository, MealRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}
