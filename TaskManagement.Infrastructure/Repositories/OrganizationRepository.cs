using AutoMapper;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class OrganizationRepository 
    : BaseRepository<Organization, OrgDetailsDto>, IOrganizationRepository
{
    public OrganizationRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) { }

}
