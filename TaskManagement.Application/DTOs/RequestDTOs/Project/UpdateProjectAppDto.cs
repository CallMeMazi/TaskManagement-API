namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public record UpdateProjectAppDto(
    int ProjId,
    int OwnerId,
    string ProjName,
    string ProjDescription
);