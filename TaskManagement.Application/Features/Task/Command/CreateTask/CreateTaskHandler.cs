using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.CreateTask;
public class CreateTaskHandler
    : IRequestHandler<CreateTaskCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public CreateTaskHandler(ITaskService taskService, IMapper mapper, IUnitOfWork uow)
    {
        _taskService = taskService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(CreateTaskCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<CreateTaskAppDto>(request);

        var createTaskRes = await _taskService.CreateTaskAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return createTaskRes;
    }
}
