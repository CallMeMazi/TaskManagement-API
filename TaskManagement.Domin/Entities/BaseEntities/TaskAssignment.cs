using TaskManagement.Common.Exceptions;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class TaskAssignment : BaseEntity
{
    public int TaskId { get; private set; }
    public int UserId { get; private set; }
    public int ProjId { get; private set; }
    public byte TotalTimeSpent { get; private set; }
    public byte StartTaskCount { get; private set; }
    public bool IsInProgress { get; private set; } = false;
    public DateTime? LastStartedAt { get; private set; }

    #region Navigation Prop

    public Task Task { get; private set; }
    public User User { get; private set; }
    public Project Project { get; private set; }
    public ICollection<TaskInfo> Info { get; private set; }

    #endregion

    private TaskAssignment() { }
    public TaskAssignment(int taskId, int userId, int projId)
    {
        ValidateTaskAssignmentCreating(taskId, userId, projId);

        TaskId = taskId;
        UserId = userId;
        ProjId = projId;
    }


    public void IncreaseTotalTimeSpent(byte totalTime)
    {
        if (totalTime <= 0)
            throw new BadRequestException("نمیتوانید کمتر از 0 به مجموع ساعت کاری این تسک اضافه کنید!");

        if (totalTime > 24)
            throw new BadRequestException("نمیتوانید بیشتر از 24 ساعت به مجموع ساعت کاری این تسک اضافه کنید!");

        TotalTimeSpent += totalTime;

        UpdatedAt = DateTime.Now;
    }
    public void IncreaseStartTaskCount()
    {
        StartTaskCount++;
    }
    public void ChangeTaskInProgress(bool isProgress)
    {
        if (IsInProgress == isProgress)
            throw new BadRequestException(isProgress ? "تسک در حال حاضر فعال است!" : "تسک در حال حاضر غیر فعال است!");

        IsInProgress = isProgress;

        if (isProgress)
        {
            LastStartedAt = DateTime.Now;
            IncreaseStartTaskCount();
        }

        UpdatedAt = DateTime.Now;
    }

    public void ValidateTaskAssignmentCreating(int taskId, int userId, int projectId)
    {
        var errorMessages = new List<string>();

        if (taskId <= 0)
            errorMessages.Add("آیدی تسک خالی است!");

        if (userId <= 0)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (projectId <= 0)
            errorMessages.Add("آیدی پروژه خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
}
