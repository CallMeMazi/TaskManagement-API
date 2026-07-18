using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Application.Features.Organization.Command.AddUserToOrg;
using TaskManagement.Application.Features.Organization.Command.ChangeOrgActivity;
using TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToAdmin;
using TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToMember;
using TaskManagement.Application.Features.Organization.Command.CreateOrg;
using TaskManagement.Application.Features.Organization.Command.DeleteOrg;
using TaskManagement.Application.Features.Organization.Command.LeaveUserFromOrg;
using TaskManagement.Application.Features.Organization.Command.RemoveUserFromOrg;
using TaskManagement.Application.Features.Organization.Command.UpdateOrg;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.OrganizationProfile;
public class OrganizationMappingProfile : Profile
{
    public OrganizationMappingProfile()
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
        CreateMap<Organization, OrgDetailsDto>();

        // MediatR Mapping
        CreateMap<CreateOrgCommand, CreateOrgAppDto>();
        CreateMap<UpdateOrgCommand, UpdateOrgAppDto>();
        CreateMap<DeleteOrgCommand, DeleteOrgAppDto>();
        CreateMap<ChangeOrgActivityCommand, ChangeActivityOrgAppDto>();
        CreateMap<AddUserToOrgCommand, AddUserOrgAppDto>();
        CreateMap<RemoveUserFromOrgCommand, RemoveUserOrgAppDto>();
        CreateMap<LeaveUserFromOrgCommand, LeaveUserOrgAppDto>();
        CreateMap<ChangeUserRoleToAdminCommand, ChangeUserRoleOrgAppDto>();
        CreateMap<ChangeUserRoleToMemberCommand, ChangeUserRoleOrgAppDto>();
    }
}
