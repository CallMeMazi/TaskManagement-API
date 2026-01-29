using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class ProjectMemberShipRepository
    : BaseRepository<ProjectMemberShip>, IProjectMemberShipRepository
{
    public ProjectMemberShipRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }
}
