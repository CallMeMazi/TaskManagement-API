using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TaskManagement.Application.Pipelines;

public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var stopWatch = Stopwatch.StartNew();

        var response = await next(ct);

        stopWatch.Stop();

        if (stopWatch.ElapsedMilliseconds > 1000)
        {
            logger.LogWarning(
                "Request.Slow {RequestName} completed in {ElapsedMilliseconds} ms",
                typeof(TRequest).Name,
                stopWatch.ElapsedMilliseconds);
        }

        return response;
    }
}
