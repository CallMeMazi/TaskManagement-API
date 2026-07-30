namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public record UserProjectAppDto(
    int OwnerId,
    string UserPassword,
    int ProjId
);