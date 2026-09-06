namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record ChangeProjectProgressAppDto(
    long OwnerId,
    long ProjId,
    byte ProjectProgress
);