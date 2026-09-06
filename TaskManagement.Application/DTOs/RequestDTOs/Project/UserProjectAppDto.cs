namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record UserProjectAppDto(
    long OwnerId,
    string UserPassword,
    long ProjId
);