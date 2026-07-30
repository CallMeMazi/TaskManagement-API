using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.UpdateTask;
public class UpdateTaskHandler
    : IRequestHandler<UpdateTaskCommand, GeneralResult>
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public UpdateTaskHandler(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(UpdateTaskCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UpdateTaskAppDto>(request);

        return _taskService.UpdateTaskAsync(dto, ct);
    }
}
