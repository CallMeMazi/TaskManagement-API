using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Project;
using TaskManagement.Application.DTOs.SharedDTOs.Project;
using TaskManagement.Domin.Entities.BaseEntities;

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
    }
}
