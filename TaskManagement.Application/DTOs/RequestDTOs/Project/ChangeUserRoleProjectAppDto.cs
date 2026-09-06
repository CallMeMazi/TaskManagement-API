namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record ChangeUserRoleProjectAppDto(
    long OwnerId,
    long ProjId,
    long UserId
);