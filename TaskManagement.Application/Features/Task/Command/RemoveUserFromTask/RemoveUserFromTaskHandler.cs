using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.RemoveUserFromTask;
public class RemoveUserFromTaskHandler
    : IRequestHandler<RemoveUserFromTaskCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public RemoveUserFromTaskHandler(ITaskService taskService, IMapper mapper, IUnitOfWork uow)
    {
        _taskService = taskService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(RemoveUserFromTaskCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<AddRemoveUserTaskAppDto>(request);

        var removeUserTaskRes = await _taskService.RemoveUserFromTaskAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return removeUserTaskRes;
    }
}
