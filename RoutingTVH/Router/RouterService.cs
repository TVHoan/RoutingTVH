using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RoutingTVH.As;


namespace RoutingTVH.Router;


public static class RouterService
{
    public static IServiceCollection AddRouterService(this IServiceCollection services)
    {
        
        services.TryAddSingleton<Refec>();
        
        return services;

    }
}