namespace TaskManagement.Domain.Interface.Repository;
public interface ITaskRepository : IBaseRepository<Entities.BaseEntities.Task>
{
    Task<int> SoftDeleteTaskSpAsync(int taskId, CancellationToken ct);
}
