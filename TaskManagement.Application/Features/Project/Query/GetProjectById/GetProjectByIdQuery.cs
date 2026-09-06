using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Project;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Query.GetProjectById;
public record GetProjectByIdQuery(long ProjectId)
    : IRequest<GeneralResult<ProjectDetailsDto>>;