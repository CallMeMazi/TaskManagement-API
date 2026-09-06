namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record CreateProjectAppDto(
    string ProjName,
    string ProjDescription,
    long OrgId,
    long CreatorId,
    byte MaxUser,
    byte MaxTask,
    List<long>? UserIds
);