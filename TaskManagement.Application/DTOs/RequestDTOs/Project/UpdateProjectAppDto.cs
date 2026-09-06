namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record UpdateProjectAppDto(
    long ProjId,
    long OwnerId,
    string ProjName,
    string ProjDescription
);