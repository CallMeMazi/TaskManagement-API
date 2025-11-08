using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services.Main;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Services.Main;
public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public OrganizationService(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    public async Task<GeneralResult> CreateOrganizationAsync(CreateOrgAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task<GeneralResult> UpdateOrganizationAsync(UpdateOrgAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task<GeneralResult> SoftDeleteOrganizationAsync(DeleteOrgAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task<GeneralResult<OrgDetailsDto>> GetOrganizationByIdCodeAsync(string orgName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task<GeneralResult> ChangeOrganizationActivityAsync(ChangeActivityOrgAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
