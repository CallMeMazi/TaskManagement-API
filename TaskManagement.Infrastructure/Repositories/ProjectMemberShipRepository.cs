using AutoMapper;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class ProjectMemberShipRepository
    : BaseRepository<ProjectMemberShip, ProjectMemberShip>, IProjectMemberShipRepository
{
    public ProjectMemberShipRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) { }
}
