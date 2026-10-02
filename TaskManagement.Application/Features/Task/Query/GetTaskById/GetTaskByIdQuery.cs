using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Task;

namespace TaskManagement.Application.Features.Task.Query.GetTaskById;
public record GetTaskByIdQuery(long TaskId)
    : IRequest<TaskDetailsDto>;