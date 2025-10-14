using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Task;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Common.Helpers;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class TaskRepository : BaseRepository<Domin.Entities.BaseEntities.Task>, ITaskRepository
{
    private readonly IMapper _mapper;

    public TaskRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext)
    {
        _mapper = mapper;
    }

    #region Async Method

    public Task<List<Domin.Entities.BaseEntities.Task>> GetAllTasksAsync(bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.ToListAsync(cancellationToken);
    }

    public Task<List<TaskDetailsDto>> GetAllTaskDtosAsync(CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .ProjectTo<TaskDetailsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public Task<Domin.Entities.BaseEntities.Task?> GetTaskByIdAsync(int taskId, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(p => p.Id == taskId, cancellationToken);
    }

    public Task<TaskDetailsDto?> GetTaskDtoByIdAsync(int taskId, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(u => u.Id == taskId)
            .ProjectTo<TaskDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Domin.Entities.BaseEntities.Task?> GetTaskByFilterAsync(Expression<Func<Domin.Entities.BaseEntities.Task, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public Task<TaskDetailsDto?> GetTaskDtoByFilterAsync(Expression<Func<Domin.Entities.BaseEntities.Task, bool>> filter, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(filter)
            .ProjectTo<TaskDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public ValueTask<Domin.Entities.BaseEntities.Task?> FindTasksByIdsAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return Entities.FindAsync(ids, cancellationToken);
    }

    public async System.Threading.Tasks.Task AddTaskAsync(Domin.Entities.BaseEntities.Task entity, CancellationToken cancellationToken = default)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddTaskAsync)} method!");

        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    public async System.Threading.Tasks.Task AddRangeTasksAsync(IEnumerable<Domin.Entities.BaseEntities.Task> entities, CancellationToken cancellationToken = default)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddRangeTasksAsync)} method!");

        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public void UpdateTask(Domin.Entities.BaseEntities.Task entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateTask)} method!");

        Entities.Update(entity);
    }

    public void UpdateRangeTasks(IEnumerable<Domin.Entities.BaseEntities.Task> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateRangeTasks)} method!");

        Entities.UpdateRange(entities);
    }

    public void DeleteTask(Domin.Entities.BaseEntities.Task entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteTask)} method!");

        Entities.Remove(entity);
    }

    public void DeleteRangeTasks(IEnumerable<Domin.Entities.BaseEntities.Task> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteRangeTasks)} method!");

        Entities.RemoveRange(entities);
    }

    #endregion
}
