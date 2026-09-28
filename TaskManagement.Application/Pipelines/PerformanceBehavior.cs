using MediatR;
using System.Diagnostics;

namespace TaskManagement.Application.Pipelines;

public class PerformanceBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var stopWatch = Stopwatch.StartNew();

        var response = await next();

        stopWatch.Stop();

        if (stopWatch.ElapsedMilliseconds > 1000)
        {
            // loging
        }

        return response;
    }
}
