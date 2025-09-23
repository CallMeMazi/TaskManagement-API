using TaskManagement.Domin.Enums.Logs;

namespace TaskManagement.Domin.Entities.LogEntities;
public class EntityRelationLog : LogBaseEntity
{
    public EntityType PrimaryEntityType { get; set; }
    public Guid PrimaryEntityId { get; set; }
    public EntityType SecondaryEntityType { get; set; }
    public Guid SecondaryEntityId { get; set; }
    public Guid ActorUserId { get; set; }
    public ActionType Action { get; set; }
}
