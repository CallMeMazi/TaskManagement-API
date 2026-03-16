using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.TaskInfo;
using TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Services.Application;
public class TaskInfoService : ITaskInfoService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;


    public TaskInfoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _uow = unitOfWork;
        _mapper = mapper;
    }


    // Query methods
    public async Task<GeneralResult<TaskInfoDetailsDto>> GetTaskInfoByIdAsync(int taskInfoId, CancellationToken ct)
    {
        var taskInfo = await _uow.TaskInfo.GetByIdAsync(taskInfoId, false, ct);

        if (taskInfo!.IsNullParameter())
            throw new NotFoundException("کاربری با این آیدی وجود ندارد!");

        var taskInfoDto = _mapper.Map<TaskInfoDetailsDto>(taskInfo);

        return GeneralResult<TaskInfoDetailsDto>.Success(taskInfoDto);
    }
    
    // Command methods
    public async Task<GeneralResult> CreateTaskInfoAsync(CreateTaskInfoAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        var taskInfo = _mapper.Map<TaskInfo>(command);

        await _uow.TaskInfo.AddAsync(taskInfo, ct);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
}
