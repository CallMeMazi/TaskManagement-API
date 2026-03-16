using TaskManagement.Common.Exceptions;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class TaskInfo : BaseEntity
{
    public int TaskId { get; private set; }
    public int UserId { get; private set; }
    public int TaskAssignmentId { get; private set; }
    public string? TaskInfoDescription { get; private set; }
    public DateTime StartedTaskAt { get; private set; }
    public DateTime? EndedTaskAt { get; private set; }
    public byte TotalHours { get; set; }

    #region Navigation Prop

    public Task Task { get; private set; }
    public User User { get; private set; }
    public TaskAssignment TaskAssignment { get; private set; }

    #endregion


    private TaskInfo() { }
    public TaskInfo(int taskId, int userId, int taskAssignmentId
        , DateTime startedTaskAt, DateTime endedTaskAt)
    {
        ValidateTaskInfo(taskId, userId, taskAssignmentId, startedTaskAt, endedTaskAt);

        TaskId = taskId;
        UserId = userId;
        TaskAssignmentId = taskAssignmentId;
        StartedTaskAt = startedTaskAt;
        EndedTaskAt = endedTaskAt;
        TotalHours = GetTotalHours();
    }


    public byte GetTotalHours() =>
        Convert.ToByte((EndedTaskAt! - StartedTaskAt).Value.TotalHours);

    public void ValidateTaskInfo(int taskId, int userId, int taskAssignmentId
        , DateTime startedTaskAt, DateTime endedTaskAt)
    {
        var errorMessages = new List<string>();

        if (taskId <= 0)
            errorMessages.Add("آیدی تسک خالی است!");

        if (userId <= 0)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (taskAssignmentId <= 0)
            errorMessages.Add("آیدی تسک کاربر خالی است!");

        if (startedTaskAt >= DateTime.Now)
            errorMessages.Add("زمان شروع تسک مربوط به آینده است!");

        if (endedTaskAt >= DateTime.Now)
            errorMessages.Add("زمان پایان تسک مربوط به آینده است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
}
