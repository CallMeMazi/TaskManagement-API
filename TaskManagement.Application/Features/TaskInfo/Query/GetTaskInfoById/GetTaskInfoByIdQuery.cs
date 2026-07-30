using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.TaskInfo;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.TaskInfo.Query.GetTaskInfoById;
public record GetTaskInfoByIdQuery(int TaskInfoId)
    : IRequest<GeneralResult<TaskInfoDetailsDto>>;