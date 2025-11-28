using AutoMapper;
using TaskManagement.Application.DTOs.SharedDTOs.Invitation;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.Invitation;
public class OrgInvitationMappingProfile : Profile
{
    public OrgInvitationMappingProfile()
    {
        // Query DTOs
        CreateMap<OrganizationInvitation, OrgInvitationDetailsDto>();
    }
}
