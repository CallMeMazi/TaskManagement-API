using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.OrganizationProfile;
public class MappingOrganizationDtoToEntity : Profile
{
    public MappingOrganizationDtoToEntity()
    {
        // Command DTOs
        CreateMap<CreateOrgAppDto, Organization>().ConstructUsing(src => 
        new Organization(
            src.OrgName,
            src.SecondOrgName,
            src.OwnerId,
            src.OrgDescription,
            src.MaxUser
        ));

        // Query DTOs
        CreateMap<Organization, OrgDetails>();
    }
}
