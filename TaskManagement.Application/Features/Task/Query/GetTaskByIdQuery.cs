using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Task;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Query;
public record GetTaskByIdQuery(int TaskId)
    : IRequest<GeneralResult<TaskDetailsDto>>;