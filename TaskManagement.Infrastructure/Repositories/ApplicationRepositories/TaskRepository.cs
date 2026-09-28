using TaskManagement.Application.Interfaces.Services.Halper;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories.ApplicationRepositories;
public class TaskRepository 
    : BaseRepository<Domain.Entities.BaseEntities.Task>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext dbContext, ICommonService commonService)
        : base(dbContext, commonService) { }


    // command methods
    public Task<int> SoftDeleteTaskSpAsync(long taskId, CancellationToken ct)
    {
        var query = string.Format("EXEC dbo.sp_SoftDeleteTask @TaskId = {0}", taskId);
        return _db.Database.ExecuteSqlRawAsync(query, ct);
    }
}
