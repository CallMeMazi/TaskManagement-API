namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public record ChangeUserRoleProjectAppDto(
    int OwnerId,
    int ProjId,
    int UserId
);