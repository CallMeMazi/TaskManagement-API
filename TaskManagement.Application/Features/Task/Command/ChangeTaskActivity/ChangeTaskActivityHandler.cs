using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskActivity;
public class ChangeTaskActivityHandler
    : IRequestHandler<ChangeTaskActivityCommand, GeneralResult>
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public ChangeTaskActivityHandler(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeTaskActivityCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeTaskActivityAppDto>(request);

        return _taskService.ChangeTaskActivityAsync(dto, ct);
    }
}
