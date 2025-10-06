using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class TaskInfoRepository : BaseRepository<TaskInfo>, ITaskInfoRepository
{
    private readonly IMapper _mapper;
    public TaskInfoRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext)
    {
        _mapper = mapper;
    }

    #region Async Method

    public Task<List<TaskInfo>> GetAllTaskInfosAsync(bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.ToListAsync(cancellationToken);
    }

    public Task<List<TaskInfoDetailsDto>> GetAllTaskInfoDtosAsync(CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .ProjectTo<TaskInfoDetailsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public Task<TaskInfo?> GetTaskInfoByIdAsync(Guid taskInfoId, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(p => p.Id == taskInfoId, cancellationToken);
    }

    public Task<TaskInfoDetailsDto?> GetTaskInfoDtoByIdAsync(Guid taskInfoId, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(u => u.Id == taskInfoId)
            .ProjectTo<TaskInfoDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TaskInfo?> GetTaskInfoByFilterAsync(Expression<Func<TaskInfo, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public Task<TaskInfoDetailsDto?> GetTaskInfoDtoByFilterAsync(Expression<Func<TaskInfo, bool>> filter, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(filter)
            .ProjectTo<TaskInfoDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public ValueTask<TaskInfo?> FindTaskInfosByIdsAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return Entities.FindAsync(ids, cancellationToken);
    }

    public async System.Threading.Tasks.Task AddTaskInfoAsync(TaskInfo entity, CancellationToken cancellationToken = default)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddTaskInfoAsync)} method!");

        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    public async System.Threading.Tasks.Task AddRangeTaskInfosAsync(IEnumerable<TaskInfo> entities, CancellationToken cancellationToken = default)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddRangeTaskInfosAsync)} method!");

        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public void UpdateTaskInfo(TaskInfo entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateTaskInfo)} method!");

        Entities.Update(entity);
    }

    public void UpdateRangeTaskInfos(IEnumerable<TaskInfo> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateRangeTaskInfos)} method!");

        Entities.UpdateRange(entities);
    }

    public void DeleteTaskInfo(TaskInfo entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteTaskInfo)} method!");

        Entities.Remove(entity);
    }

    public void DeleteRangeTaskInfos(IEnumerable<TaskInfo> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteRangeTaskInfos)} method!");

        Entities.RemoveRange(entities);
    }

    #endregion
}
