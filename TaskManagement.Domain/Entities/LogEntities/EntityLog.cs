using TaskManagement.Domain.Enums.Logs;

namespace TaskManagement.Domain.Entities.LogEntities;
public class EntityLog : LogBaseEntity
{
    public EntityType EntityType { get; set; }
    public int EntityId { get; set; }
    public ActionType Action { get; set; }
}
