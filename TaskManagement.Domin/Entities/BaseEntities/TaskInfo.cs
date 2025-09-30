using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class TaskInfo : BaseEntity
{
    public Guid TaskId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid TaskAssignmentId { get; private set; }
    public string TaskInfoDescreption { get; private set; }
    public DateTime StartedTaskAt { get; private set; }
    public DateTime EndedTaskAt { get; private set; } = DateTime.Now;
    public byte TotalHours => Convert.ToByte((EndedTaskAt - StartedTaskAt).TotalHours);

    #region Navigation Prop

    public Task Task { get; private set; }
    public User User { get; private set; }
    public TaskAssignment TaskAssignment { get; private set; }

    #endregion


    private TaskInfo() { }
    public TaskInfo(Guid taskId, Guid userId, Guid taskAssignmentId
        , string taskInfoDescreption, DateTime startedTaskAt)
    {
        ValidateTaskInfo(taskId, userId, taskAssignmentId, taskInfoDescreption, startedTaskAt);

        TaskId = taskId;
        UserId = userId;
        TaskAssignmentId = taskAssignmentId;
        TaskInfoDescreption = taskInfoDescreption;
        StartedTaskAt = startedTaskAt;
    }


    public void ValidateTaskInfo(Guid taskId, Guid userId, Guid taskAssignmentId
        , string taskInfoDescreption, DateTime startedTaskAt)
    {
        var errorMessages = new List<string>();

        if (taskId == Guid.Empty)
            errorMessages.Add("آیدی تسک خالی است!");

        if (userId == Guid.Empty)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (taskAssignmentId == Guid.Empty)
            errorMessages.Add("آیدی تسک کاربر خالی است!");

        if (taskInfoDescreption.IsNullParameter())
            errorMessages.Add("توضیحات گزارش تسک خالی است!");

        if (startedTaskAt.IsNullParameter())
            errorMessages.Add("تاریخ شروع خالی است!");

        if (startedTaskAt >= DateTime.Now)
            errorMessages.Add("تاریخ شروع نامعتبر است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
}
