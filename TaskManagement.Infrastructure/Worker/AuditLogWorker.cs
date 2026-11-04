using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Domain.Entities.LogEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Worker;

public class AuditLogWorker(
    IAuditQueue queue,
    IServiceScopeFactory scopeFactory,
    ILogger<AuditLogWorker> logger)
    : BackgroundService
{
    private const int maxBatchSize = 100;
    private const int maxAttempts = 3;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"Audit Worker Started At {DateTime.Now}.");

        while (!stoppingToken.IsCancellationRequested)
        {
            List<AuditLog> batch;

            try
            {
                batch = await ReadAllAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            await SendBatchAsync(batch, stoppingToken);
        }

        logger.LogInformation($"Audit Worker Ended At {DateTime.Now}");
    }

    private async Task<List<AuditLog>> ReadAllAsync(CancellationToken ct)
    {
        var audits = new List<AuditLog>(maxBatchSize);

        var batchTime = CancellationTokenSource.CreateLinkedTokenSource(ct);
        batchTime.CancelAfter(1000);

        while (audits.Count < maxBatchSize)
        {
            try
            {
                var audit = await queue.Reader().ReadAsync(batchTime.Token);
                audits.Add(audit);
            }
            catch (OperationCanceledException) when (batchTime.IsCancellationRequested)
            {
                break;
            }
        }

        return audits;
    }
    private async Task SendBatchAsync(List<AuditLog> batch, CancellationToken ct)
    {
        var lastException = new Exception();

        for (int attempt = 0; attempt <= maxAttempts; attempt++)
        {
            {
                try
                {
                    await using var scope = scopeFactory.CreateAsyncScope();

                    var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();

                    db.AddRange(batch);
                    await db.SaveChangesAsync(ct);

                    logger.LogInformation($"Successfully Registered {batch.Count} Audits At {DateTime.Now}");

                    return;
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    logger.LogWarning($"Token Was Canceled At {DateTime.Now}");

                    throw;
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    if (attempt == maxAttempts)
                        break;

                    var delaySeconds = 1 * Math.Pow(2, attempt - 1);
                    var delay = TimeSpan.FromSeconds(delaySeconds);

                    logger.LogWarning(ex, $"Audit batch persistence failed. Attempt {attempt}/{maxAttempts}. Retrying after {delaySeconds} seconds.");

                    await Task.Delay(delay, ct);
                }
            }

            logger.LogError(lastException, $"Audit batch permanently failed after {maxAttempts} attempts. {batch.Count} entries were dropped.");
        }
    }
}
