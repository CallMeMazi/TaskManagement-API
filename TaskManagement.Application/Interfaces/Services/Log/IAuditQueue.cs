using System.Threading.Channels;
using TaskManagement.Domain.Entities.LogEntities;

namespace TaskManagement.Application.Interfaces.Services.Log;

public interface IAuditQueue
{
    bool TryEnqueue(AuditLog audit);
    ChannelReader<AuditLog> Reader();
}
