namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public record ChangeProjectProgressAppDto(
    int OwnerId,
    int ProjId,
    byte ProjectProgress
);