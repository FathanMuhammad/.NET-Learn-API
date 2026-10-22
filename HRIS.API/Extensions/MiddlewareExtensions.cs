using HRIS.API.Middlewares;

namespace HRIS.API.Extensions;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddHrisMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<RequestTimingMiddleware>();
        services.AddTransient<MaintenanceModeMidleware>();
        return services;
    }

    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder builder)
        => builder.UseMiddleware<RequestTimingMiddleware>();

    public static IApplicationBuilder UseMaintenanceMode(this IApplicationBuilder builder)
        => builder.UseMiddleware<MaintenanceModeMidleware>();
}
