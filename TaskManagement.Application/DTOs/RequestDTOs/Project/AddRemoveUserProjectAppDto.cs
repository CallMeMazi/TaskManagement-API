namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public record AddRemoveUserProjectAppDto(
    int UserId,
    int ProjId,
    int OwnerId
);