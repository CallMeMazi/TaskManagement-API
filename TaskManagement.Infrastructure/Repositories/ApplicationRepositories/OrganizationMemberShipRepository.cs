using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories.ApplicationRepositories;
public class OrganizationMemberShipRepository 
    : BaseRepository<OrganizationMemberShip>, IOrganizationMemberShipRepository
{
    public OrganizationMemberShipRepository(ApplicationDbContext dbContext, ICommonService commonService)
        : base(dbContext, commonService) { }
}
