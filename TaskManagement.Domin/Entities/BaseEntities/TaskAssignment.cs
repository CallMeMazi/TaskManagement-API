using TaskManagement.Common.Exceptions;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class TaskAssignment : BaseEntity
{
    public Guid TaskId { get; private set; }
    public Guid UserId { get; private set; }
    public byte TotalTimeSpent { get; private set; }
    public byte StartTaskCount { get; private set; }
    public bool IsInProgress { get; private set; } = false;
    public DateTime? LastStartedAt { get; private set; }

    #region Navigation Prop

    public Task Task { get; private set; }
    public User User { get; private set; }
    public ICollection<TaskInfo> Info { get; private set; }

    #endregion

    private TaskAssignment() { }
    public TaskAssignment(Guid taskId, Guid userId)
    {
        ValidateTaskAssignmentCreating(taskId, userId);

        TaskId = taskId;
        UserId = userId;
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
        => StartTaskCount++;
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

    public void ValidateTaskAssignmentCreating(Guid taskId, Guid userId)
    {
        var errorMessages = new List<string>();

        if (taskId == Guid.Empty)
            errorMessages.Add("آیدی تسک خالی است!");

        if (userId == Guid.Empty)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
}
