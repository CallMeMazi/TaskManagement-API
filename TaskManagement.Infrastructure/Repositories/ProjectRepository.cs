using AutoMapper;
using TaskManagement.Application.DTOs.SharedDTOs.Project;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class ProjectRepository 
    : BaseRepository<Project, ProjectDetailsDto>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) { }
}
