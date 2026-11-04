using System.Threading.Channels;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Domain.Entities.LogEntities;

namespace TaskManagement.Infrastructure.Queues;

public class AuditQueue : IAuditQueue
{
    private readonly Channel<AuditLog> _channel = Channel.CreateBounded<AuditLog>(
        new BoundedChannelOptions(capacity: 5000)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        }
    );

    public bool TryEnqueue(AuditLog audit)
        => _channel.Writer.TryWrite(audit);

    public ChannelReader<AuditLog> Reader()
        => _channel.Reader;
}
