using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskProgress;
public class ChangeTaskProgressHandler
    : IRequestHandler<ChangeTaskProgressCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public ChangeTaskProgressHandler(ITaskService taskService, IMapper mapper, IUnitOfWork uow)
    {
        _taskService = taskService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeTaskProgressCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeTaskProgressAppDto>(request);

        var changeTaskProgressRes = await _taskService.ChangeTaskProgressAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return changeTaskProgressRes;
    }
}
