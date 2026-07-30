using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.DeleteTask;
public class DeleteTaskHandler
    : IRequestHandler<DeleteTaskCommand, GeneralResult>
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public DeleteTaskHandler(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UserTaskAppDto>(request);

        return _taskService.SoftDeleteTaskAsync(dto, ct);
    }
}
