using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Enums;
using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class Task : BaseEntity
{
    public int ProjId { get; private set; }
    public int CreatorId { get; private set; }
    public string TaskName { get; private set; }
    public string TaskDescription { get; private set; }
    public bool IsActive { get; private set; } = true;
    public TaskType TaskType { get; private set; }
    public TaskStatusType TaskStatus { get; private set; } = TaskStatusType.InProgress;
    public DateTime TaskDeadline { get; private set; }
    public byte TaskProgress { get; private set; }

    #region Navigation Prop

    public Project Project { get; private set; }
    public User Creator { get; private set; }
    public ICollection<TaskAssignment> Members { get; private set; }
    public ICollection<TaskInfo> Info { get; private set; }

    #endregion


    private Task() { }
    public Task(int projId, int creatorId, string taskName
        , string taskDescription, TaskType taskType, DateTime taskDeadline)
    {
        ValidateTaskCreating(projId, creatorId, taskName, taskDescription, taskDeadline);

        ProjId = projId;
        CreatorId = creatorId;
        TaskName = taskName;
        TaskDescription = taskDescription;
        TaskType = taskType;
        TaskDeadline = taskDeadline;
    }


    public void UpdateTask(string taskName, string taskDescription, DateTime taskDeadline)
    {
        ValidateTaskUpdating(taskName, taskDescription, taskDeadline);

        TaskName = taskName;
        TaskDescription = taskDescription;
        TaskDeadline = taskDeadline;

        UpdatedAt = DateTime.Now;
    }
    public void ChangeTaskActivity(bool activity)
    {
        if (TaskStatus == TaskStatusType.Cancel)
            throw new BadRequestException("تسک کنسل شده، نمیتوانید وضعیت آن را تغییر دهید!");

        if (TaskStatus == TaskStatusType.Dead)
            throw new BadRequestException("زمان تسک به اتمام رسیده، نمیتوانید وضعیت آن را تغییر دهید!");

        if (TaskStatus == TaskStatusType.Finished)
            throw new BadRequestException("تسک به پایان رسیده، نمیتوانید وضعیت آن را تغییر دهید!");

        if (IsActive == activity)
            throw new BadRequestException(IsActive ? "تسک در حال حاضر فعال است!" : "تسک در حال حاضر غیر فعال است!");

        IsActive = activity;

        UpdatedAt = DateTime.Now;
    }
    public void CancelTask()
    {
        if (TaskStatus == TaskStatusType.Cancel)
            return;

        if (TaskStatus == TaskStatusType.Dead)
            throw new BadRequestException("زمان تسک اتمام رسیده، نمیتوانید وضعیت آن را تغییر دهید!");

        if (TaskStatus == TaskStatusType.Finished)
            throw new BadRequestException("تسک به اتمام رسیده، نمیتوانید وضعیتت آن را تغییر دهید!");

        TaskStatus = TaskStatusType.Cancel;
        IsActive = false;

        UpdatedAt = DateTime.Now;
    }
    public void FinishTask()
    {
        if (TaskStatus == TaskStatusType.Finished)
            return;

        if (TaskStatus == TaskStatusType.Dead)
            throw new BadRequestException("زمان تسک اتمام رسیده، نمیتوانید وضعیت آن را تغییر دهید!");

        if (TaskStatus == TaskStatusType.Cancel)
            throw new BadRequestException("تسک کنسل شده است، نمیتوانید وضعیتت آن را تغییر دهید!");

        TaskStatus = TaskStatusType.Finished;
        TaskProgress = 100;
        IsActive = false;

        UpdatedAt = DateTime.Now;
    }
    public void DeadTask()
    {
        if (TaskStatus == TaskStatusType.Finished)
            return;

        if (TaskDeadline <= DateTime.Now)
        {
            TaskStatus = TaskStatusType.Dead;
            IsActive = false;
            UpdatedAt = DateTime.Now;
        }
    }
    public void ChangeTaskProgress(byte progress)
    {
        if (!IsActive)
            throw new BadRequestException("تسک فعال نیست، نمیتوانید میزان پیشرفت را تغییر دهید!");

        if (TaskProgress == progress)
            return;

        if (progress <= 0 || progress > 100)
            throw new BadRequestException("نمیتوانید میزان پیشرفت را کمتر از 0 و بیشتر از 100 وارد کنید!");

        if (TaskProgress > progress)
            throw new BadRequestException("نمیتوانید میزان پیشرفت را کمتر از میزان فعلی اعمال کنید!");

        TaskProgress = progress;

        if (progress == 100)
        {
            IsActive = false;
            FinishTask();
        }

        UpdatedAt = DateTime.Now;
    }

    public void ValidateTaskCreating(int projId, int creatorId, string taskName
        , string taskDescription, DateTime taskDeadline)
    {
        var errorMessages = new List<string>();

        if (projId <= 0)
            errorMessages.Add("آیدی پروژه خالی است!");

        if (creatorId <= 0)
            errorMessages.Add("آیدی کاربر سازنده خالی است!");

        if (taskName.IsNullParameter())
            errorMessages.Add("نام تسک خالی است!");

        if (taskDescription.IsNullParameter())
            errorMessages.Add("توضیحات تسک خالی است!");

        if (taskDeadline.IsNullParameter())
            errorMessages.Add("مهلت پایان تسک خالی است!");

        if (taskDeadline <= DateTime.Now)
            errorMessages.Add("مهلت پایان تسک مربوط به گذشته است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
    public void ValidateTaskUpdating(string taskName, string taskDescription, DateTime taskDeadline)
    {
        var errorMessages = new List<string>();

        if (taskName.IsNullParameter())
            errorMessages.Add("نام تسک خالی است!");

        if (taskDescription.IsNullParameter())
            errorMessages.Add("توضیحات تسک خالی است!");

        if (taskDeadline.IsNullParameter())
            errorMessages.Add("مهلت پایان تسک خالی است!");

        if (taskDeadline <= DateTime.Now)
            errorMessages.Add("مهلت پایان تسک مربوط به گذشته است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
}
