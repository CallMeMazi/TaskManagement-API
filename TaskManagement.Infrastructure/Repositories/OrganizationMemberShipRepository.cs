using AutoMapper;
using TaskManagement.Application.DTOs.SharedDTOs.OrganizationMemberShip;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class OrganizationMemberShipRepository 
    : BaseRepository<OrganizationMemberShip, OrgMemberShipDetailsDto>, IOrganizationMemberShipRepository
{
    public OrganizationMemberShipRepository(ApplicationDbContext dbContext, IMapper mapper) 
        : base(dbContext, mapper) { }
}
