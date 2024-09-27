using Microsoft.AspNetCore.Builder;

namespace RoutingTVH.Router;

public static class Router
{
    public static IApplicationBuilder UseRouter(this IApplicationBuilder builder)
    { 
        builder.UseMiddleware<RouterMiddleware>();
        return builder;
    }
}