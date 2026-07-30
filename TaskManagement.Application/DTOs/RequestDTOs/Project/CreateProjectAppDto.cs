namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record CreateProjectAppDto(
    string ProjName,
    string ProjDescription,
    int OrgId,
    int CreatorId,
    byte MaxUser,
    byte MaxTask,
    List<int>? UserIds
);