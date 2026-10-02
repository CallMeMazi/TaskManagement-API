using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Project;

namespace TaskManagement.Application.Features.Project.Query.GetProjectById;
public record GetProjectByIdQuery(long ProjectId)
    : IRequest<ProjectDetailsDto>;