using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class TaskInfo : BaseEntity
{
    public Guid TaskId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid TaskAssignmentId { get; private set; }
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
    public TaskInfo(Guid taskId, Guid userId, Guid taskAssignmentId)
    {
        ValidateTaskInfo(taskId, userId, taskAssignmentId);

        TaskId = taskId;
        UserId = userId;
        TaskAssignmentId = taskAssignmentId;
    }


    public void EndTask(string taskInfoDescription)
    {
        if (taskInfoDescription.IsNullParameter())
            throw new BadRequestException("توضیحات تسک خالی است!");
       
        TaskInfoDescription = taskInfoDescription;
        EndedTaskAt = DateTime.Now;
        TotalHours = GetTotalHours(StartedTaskAt);

        UpdatedAt = DateTime.Now;
    }

    public void ValidateTaskInfo(Guid taskId, Guid userId, Guid taskAssignmentId)
    {
        var errorMessages = new List<string>();

        if (taskId == Guid.Empty)
            errorMessages.Add("آیدی تسک خالی است!");

        if (userId == Guid.Empty)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (taskAssignmentId == Guid.Empty)
            errorMessages.Add("آیدی تسک کاربر خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
    public byte GetTotalHours(DateTime startrdTaskAt) =>
        Convert.ToByte((EndedTaskAt! - startrdTaskAt).Value.TotalHours);
}
