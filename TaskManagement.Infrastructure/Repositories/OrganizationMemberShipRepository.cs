using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class OrganizationMemberShipRepository 
    : BaseRepository<OrganizationMemberShip>, IOrganizationMemberShipRepository
{
    public OrganizationMemberShipRepository(ApplicationDbContext dbContext) 
        : base(dbContext) { }
}
