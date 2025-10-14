using TaskManagement.Domin.Enums.Logs;

namespace TaskManagement.Domin.Entities.LogEntities;
public class EntityLog : LogBaseEntity
{
    public EntityType EntityType { get; set; }
    public int EntityId { get; set; }
    public ActionType Action { get; set; }
}
