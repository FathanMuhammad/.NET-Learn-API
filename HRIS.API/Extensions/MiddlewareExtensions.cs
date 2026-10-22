using HRIS.API.Middlewares;

namespace HRIS.API.Extensions;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddHrisMiddlewares(this IServiceCollection services)
    {
        services.AddSingleton<RequestTimingMiddleware>();       //Middleware kalo bisa singleton karena ga perlu diinstansiasi berkali2, cukup 1 instance aja
        services.AddSingleton<MaintenanceModeMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder builder)
        => builder.UseMiddleware<RequestTimingMiddleware>();

    public static IApplicationBuilder UseMaintenanceMode(this IApplicationBuilder builder)
        => builder.UseMiddleware<MaintenanceModeMiddleware>();
}
