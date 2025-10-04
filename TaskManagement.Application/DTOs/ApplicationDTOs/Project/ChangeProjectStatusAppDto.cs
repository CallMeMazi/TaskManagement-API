using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public class ChangeProjectStatusAppDto : UserProjectAppDto
{
    public ProjectStatusType ProjectStatus { get; set; }
}
