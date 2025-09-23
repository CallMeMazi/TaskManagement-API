namespace TaskManagement.Domin.Entities.LogEntities;
public abstract class LogBaseEntity
{
    public Guid Id { get; set; }
    public required string LogDescription { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.Now;
}
