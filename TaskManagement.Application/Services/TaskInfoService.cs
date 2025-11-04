using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.TaskInfo;
using TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Services.Main;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Services;
public class TaskInfoService : ITaskInfoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public TaskInfoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<GeneralResult> CreateTaskInfoAsync(CreateTaskInfoAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> EndTaksInfoAsync(EndTaskInfoAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult<TaskInfoDetailsDto>> GetTaskInfoByIdAsync(int taskInfoId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
