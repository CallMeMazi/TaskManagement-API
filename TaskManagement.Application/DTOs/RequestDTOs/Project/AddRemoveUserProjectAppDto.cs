namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record AddRemoveUserProjectAppDto(
    int UserId,
    int ProjId,
    int OwnerId
);