using Microsoft.EntityFrameworkCore;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class TaskRepository 
    : BaseRepository<Domin.Entities.BaseEntities.Task>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }


    // command methods
    public Task<int> SoftDeleteTaskSpAsync(int taskId, CancellationToken ct)
    {
        var query = string.Format("EXEC dbo.sp_SoftDeleteTask @TaskId = {0}", taskId);
        return _db.Database.ExecuteSqlRawAsync(query, ct);
    }
}
