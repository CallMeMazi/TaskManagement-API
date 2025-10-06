using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Application.DTOs.SharedDTOs.Project;
public class ProjectDetailsDto
{
    public required string ProjName { get; set; }
    public required string ProjDescription { get; set; }
    public byte ProjProgress { get; set; }
    public ProjectStatusType ProjStatus { get; set; }
    public bool IsActive { get; set; }
    public DateTime ProjStartAt { get; private set; }
    public DateTime? ProjEndAt { get; private set; }
    public byte ProjMaxUsers { get; private set; }
    public byte ProjMaxTasks { get; private set; }
    public DateTime CreateAt { get; set; }
}
