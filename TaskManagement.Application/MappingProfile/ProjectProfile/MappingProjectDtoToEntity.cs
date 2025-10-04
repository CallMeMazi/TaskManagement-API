using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Project;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.ProjectProfile;
public class MappingProjectDtoToEntity : Profile
{
    public MappingProjectDtoToEntity()
    {
        CreateMap<CreateProjectAppDto, Project>().ConstructUsing(src =>
        new Project(
            src.ProjName,
            src.ProjDescription,
            src.OrgId,
            src.CreatorId,
            src.MaxUser,
            src.MaxTask
        ));
    }
}
