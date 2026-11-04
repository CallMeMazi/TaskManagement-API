namespace TaskManagement.Domain.Enums.Types.Logs;

// Persisted audit rows are successful commits only.
// Expected business failures are not written, because a rolled-back transaction cannot keep a Failed row.
public enum AuditOutcome : byte
{
    Succeeded = 1
}
