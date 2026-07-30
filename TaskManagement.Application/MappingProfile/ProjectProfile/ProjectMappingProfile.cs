using AutoMapper;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.DTOs.ResponseDTOs.Project;
using TaskManagement.Application.Features.Project.Command.AddUserToProject;
using TaskManagement.Application.Features.Project.Command.ChagneProjectStatus;
using TaskManagement.Application.Features.Project.Command.ChangeProjectActivity;
using TaskManagement.Application.Features.Project.Command.ChangeProjectProgress;
using TaskManagement.Application.Features.Project.Command.ChangeUserRoleToAdmin;
using TaskManagement.Application.Features.Project.Command.ChangeUserRoleToMember;
using TaskManagement.Application.Features.Project.Command.CreateProject;
using TaskManagement.Application.Features.Project.Command.DeleteProject;
using TaskManagement.Application.Features.Project.Command.RemoveUserFromProject;
using TaskManagement.Application.Features.Project.Command.UpdateProject;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.ProjectProfile;
public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        // Command DTOs
        CreateMap<CreateProjectAppDto, Project>().ConstructUsing(src =>
        new Project(
            src.ProjName,
            src.ProjDescription,
            src.OrgId,
            src.CreatorId,
            src.MaxUser,
            src.MaxTask
        ));

        // Query DTOs
        CreateMap<Project, ProjectDetailsDto>();

        // MediatR Mapping
        CreateMap<CreateProjectCommand, CreateProjectAppDto>();
        CreateMap<UpdateProjectCommand, UpdateProjectAppDto>();
        CreateMap<DeleteProjectCommand, UserProjectAppDto>();
        CreateMap<ChangeProjectActivityCommand, ChangeProjectActivityAppDto>();
        CreateMap<ChagneProjectStatusCommand, UserProjectAppDto>();
        CreateMap<ChangeProjectProgressCommand, ChangeProjectProgressAppDto>();
        CreateMap<AddUserToProjectCommand, AddRemoveUserProjectAppDto>();
        CreateMap<RemoveUserFromProjectCommand, AddRemoveUserProjectAppDto>();
        CreateMap<ChangeUserRoleToAdminCommand, ChangeUserRoleProjectAppDto>();
        CreateMap<ChangeUserRoleToMemberCommand, ChangeUserRoleProjectAppDto>();
    }
}
