namespace TaskManagement.Domain.Entities.LogEntities;

// Schema owner for the Serilog operational log table. The application does not write this entity.
// Serilog.Sinks.MSSqlServer inserts Warning and Error rows. The table is not tamper-proof and has no retention job.
public class OperationalLog
{
    public int Id { get; set; }
    public string? Message { get; set; }
    public string? MessageTemplate { get; set; }
    public string? Level { get; set; }
    public DateTime TimeStamp { get; set; }
    public string? Exception { get; set; }
    public string? Properties { get; set; }
    public string? TraceId { get; set; }
    public string? SourceContext { get; set; }
}
