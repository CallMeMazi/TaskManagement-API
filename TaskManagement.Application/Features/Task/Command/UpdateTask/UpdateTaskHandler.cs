using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.UpdateTask;
public class UpdateTaskHandler
    : IRequestHandler<UpdateTaskCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public UpdateTaskHandler(ITaskService taskService, IMapper mapper, IUnitOfWork uow)
    {
        _taskService = taskService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(UpdateTaskCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UpdateTaskAppDto>(request);

        var updateTaskRes = await _taskService.UpdateTaskAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return updateTaskRes;
    }
}
