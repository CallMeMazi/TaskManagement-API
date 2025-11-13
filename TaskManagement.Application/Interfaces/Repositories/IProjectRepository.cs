using TaskManagement.Application.DTOs.SharedDTOs.Project;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IProjectRepository : IBaseRepository<Project, ProjectDetailsDto>
{
}

