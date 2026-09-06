namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record AddRemoveUserProjectAppDto(
    long UserId,
    long ProjId,
    long OwnerId
);