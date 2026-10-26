using System.Diagnostics;

namespace HRIS.API.Middlewares;

public class RequestTimingMiddleware(ILogger<RequestTimingMiddleware> logger) : IMiddleware
{
    private readonly string _correlationHeader = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var correlationId = context.Request.Headers[_correlationHeader].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }
        
        context.Response.Headers[_correlationHeader] = correlationId;

        // Module 7: UseExceptionHandler clears the response headers before it writes an error response.
        // Keep the id in Items and put the header back right before the response starts.
        context.Items[_correlationHeader] = correlationId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[_correlationHeader] = correlationId;
            return Task.CompletedTask;
        });

        using (logger.BeginScope("CorrelationId: {CorrelationId}", correlationId))
        {
            await next(context);
        }

        stopwatch.Stop();
        logger.LogDebug("{Method} {Path} responded {StatusCode} in {ElapsedTime} ms", 
            context.Request.Method, 
            context.Request.Path, 
            context.Response.StatusCode, 
            stopwatch.ElapsedMilliseconds);
    }
}
