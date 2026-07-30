using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.CreateTask;
public class CreateTaskHandler
    : IRequestHandler<CreateTaskCommand, GeneralResult>
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public CreateTaskHandler(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(CreateTaskCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<CreateTaskAppDto>(request);

        return _taskService.CreateTaskAsync(dto, ct);
    }
}
