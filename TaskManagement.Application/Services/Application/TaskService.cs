using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Task;
using TaskManagement.Application.DTOs.SharedDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Services.Application;
public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<GeneralResult> ChangeTaskActivityAsync(ChangeTaskActivityAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> ChangeTaskStatusAsync(ChangeTaskStatusAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> CreateTaskAsync(CreateTaskAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult<TaskDetailsDto>> GetTaskByIdAsync(int taskId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> SoftDeleteTaskAsync(UserTaskAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> UpdateTaskAsync(UpdateTaskAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
