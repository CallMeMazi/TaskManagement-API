using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Project;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Query.GetProjectById;
public record GetProjectByIdQuery(int ProjectId)
    : IRequest<GeneralResult<ProjectDetailsDto>>;