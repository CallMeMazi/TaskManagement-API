using TaskManagement.Domain.Enums.Logs;

namespace TaskManagement.Domain.Entities.LogEntities;
public class EntityRelationLog : LogBaseEntity
{
    public EntityType PrimaryEntityType { get; set; }
    public int PrimaryEntityId { get; set; }
    public EntityType SecondaryEntityType { get; set; }
    public int SecondaryEntityId { get; set; }
    public int ActorUserId { get; set; }
    public ActionType Action { get; set; }
}
