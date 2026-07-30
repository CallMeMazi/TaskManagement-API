namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record UpdateProjectAppDto(
    int ProjId,
    int OwnerId,
    string ProjName,
    string ProjDescription
);