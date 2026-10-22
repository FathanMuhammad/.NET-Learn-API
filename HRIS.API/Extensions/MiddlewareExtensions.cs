using HRIS.API.Middlewares;

namespace HRIS.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder builder)
        => builder.UseMiddleware<RequestTimingMiddleware>();
}
