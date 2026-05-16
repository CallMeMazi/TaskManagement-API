using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class ProjectMemberShipRepository
    : BaseRepository<ProjectMemberShip>, IProjectMemberShipRepository
{
    public ProjectMemberShipRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }
}
