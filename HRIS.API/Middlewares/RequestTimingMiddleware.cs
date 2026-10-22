using System.Diagnostics;

namespace HRIS.API.Middlewares;

public class RequestTimingMiddleware(ILogger<RequestTimingMiddleware> _logger) : IMiddleware
{
    private readonly string CorrelationHeader = "X-Correlation-ID";
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var correlationId = context.Request.Headers[CorrelationHeader].FirstOrDefault();
        if (string.IsNullOrEmpty(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }
        context.Response.Headers[CorrelationHeader] = correlationId;
        using (_logger.BeginScope("CorrelationId: {CorrelationId}", correlationId))
        {
            await next(context);
        }
        stopwatch.Stop();
        _logger.LogInformation("Request {Method} {Path} responded {StatusCode} executed in {ElapsedMilliseconds} ms [CorrelationId: {CorrelationId}]",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            correlationId);
    }
}


