using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class OrganizationMemberShipRepository 
    : BaseRepository<OrganizationMemberShip>, IOrganizationMemberShipRepository
{
    public OrganizationMemberShipRepository(ApplicationDbContext dbContext) 
        : base(dbContext) { }
}
