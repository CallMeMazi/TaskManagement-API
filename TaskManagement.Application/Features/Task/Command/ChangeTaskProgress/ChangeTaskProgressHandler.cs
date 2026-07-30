using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskProgress;
public class ChangeTaskProgressHandler
    : IRequestHandler<ChangeTaskProgressCommand, GeneralResult>
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public ChangeTaskProgressHandler(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeTaskProgressCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeTaskProgressAppDto>(request);

        return _taskService.ChangeTaskProgressAsync(dto, ct);
    }
}
