namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record ChangeProjectProgressAppDto(
    int OwnerId,
    int ProjId,
    byte ProjectProgress
);