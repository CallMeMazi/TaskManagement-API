using TaskManagement.Application.DTOs.InternalDTOs.Common;

namespace TaskManagement.Application.Interfaces.Services.Log;

public interface IAuditLogService
{
    void Stage(AuditEntryInternalDto entry);
}
