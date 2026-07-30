namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record ChangeUserRoleProjectAppDto(
    int OwnerId,
    int ProjId,
    int UserId
);