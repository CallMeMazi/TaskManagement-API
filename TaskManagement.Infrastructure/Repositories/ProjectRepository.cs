using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Project;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    private readonly IMapper _mapper;

    public ProjectRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext)
    {
        _mapper = mapper;
    }

    #region Async Method

    public Task<List<Project>> GetAllProjectsAsync(bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.ToListAsync(cancellationToken);
    }

    public Task<List<ProjectDetailsDto>> GetAllProjectDtosAsync(CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .ProjectTo<ProjectDetailsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public Task<Project?> GetProjectByIdAsync(Guid projectId, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
    }

    public Task<ProjectDetailsDto?> GetProjectDtoByIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(u => u.Id == projectId)
            .ProjectTo<ProjectDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Project?> GetProjectByFilterAsync(Expression<Func<Project, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public Task<ProjectDetailsDto?> GetProjectDtoByFilterAsync(Expression<Func<Project, bool>> filter, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(filter)
            .ProjectTo<ProjectDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public ValueTask<Project?> FindProjectsByIdsAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return Entities.FindAsync(ids, cancellationToken);
    }

    public async System.Threading.Tasks.Task AddProjectAsync(Project entity, CancellationToken cancellationToken = default)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddProjectAsync)} method!");

        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    public async System.Threading.Tasks.Task AddRangeProjectsAsync(IEnumerable<Project> entities, CancellationToken cancellationToken = default)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddRangeProjectsAsync)} method!");

        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public void UpdateProject(Project entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateProject)} method!");

        Entities.Update(entity);
    }

    public void UpdateRangeProjects(IEnumerable<Project> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateRangeProjects)} method!");

        Entities.UpdateRange(entities);
    }

    public void DeleteProject(Project entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteProject)} method!");

        Entities.Remove(entity);
    }

    public void DeleteRangeProjects(IEnumerable<Project> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteRangeProjects)} method!");

        Entities.RemoveRange(entities);
    }

    #endregion
}
