namespace TaskManagement.Domain.Interface.Repository;
public interface ITaskRepository : IBaseRepository<Entities.BaseEntities.Task>
{
    Task<int> SoftDeleteTaskSpAsync(long taskId, CancellationToken ct);
}
