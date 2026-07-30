using AutoMapper;
using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
using TaskManagement.Application.Features.Invitation.Command.AcceptInvitation;
using TaskManagement.Application.Features.Invitation.Command.GenerateInviteLinkByUserId;
using TaskManagement.Application.Features.Invitation.Command.RevokeInvitation;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.InvitationProfile;
public class OrgInvitationMappingProfile : Profile
{
    public OrgInvitationMappingProfile()
    {
        // Query DTOs
        CreateMap<OrganizationInvitation, OrgInvitationDetailsDto>();

        // MediatR Mapping
        CreateMap<CreateOrgInvitatoinAppDto, GenerateInviteLinkByUserIdCommand>();
        CreateMap<AcceptOrgInvitationAppDto, AcceptInvitationCommand>();
        CreateMap<RevokeOrgInvitationAppDto, RevokeInvitationCommand>();
    }
}
