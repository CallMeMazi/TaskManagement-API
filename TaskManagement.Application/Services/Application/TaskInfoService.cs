using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.DTOs.ResponseDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Application.Utilities.Exceptions;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Application.Services.Application;
public class TaskInfoService : ITaskInfoService
{
    private readonly IUnitOfWork _uow;
    private readonly ICommonService _common;

    public TaskInfoService(IUnitOfWork unitOfWork, ICommonService common)
    {
        _uow = unitOfWork;
        _common = common;
    }

    // Query methods
    public async Task<GeneralResult<TaskInfoDetailsDto>> GetTaskInfoByIdAsync(long taskInfoId, CancellationToken ct)
    {
        var taskInfo = await _uow.TaskInfo.GetByIdAsync(taskInfoId, false, ct);

        if (taskInfo!.IsNullParameter())
            throw new NotFoundException("کاربری با این آیدی وجود ندارد!");

        var taskInfoDto = _common.Mapper.Map<TaskInfoDetailsDto>(taskInfo);

        return GeneralResult<TaskInfoDetailsDto>.Success(taskInfoDto);
    }

    // Command methods
    public async Task<GeneralResult> CreateTaskInfoAsync(CreateTaskInfoAppDto command, CancellationToken ct)
    {
        var taskAssignment = await _uow.TaskAssignment.GetByFilterAsync(ta =>
            ta.TaskId == command.TaskId
            && ta.UserId == command.UserId,
            false,
            ct
        );
        if (taskAssignment.IsNullParameter())
            throw new NotFoundException("اطلاعات نامعتبر است!");

        var taskInfo = _common.Mapper.Map<TaskInfo>(taskAssignment, opt => opt.Items[nameof(TaskInfo.TaskInfoDescription)] = command.TaskInfoDescription);

        await _uow.TaskInfo.AddAsync(taskInfo, ct);

        return GeneralResult.Success();
    }
}
