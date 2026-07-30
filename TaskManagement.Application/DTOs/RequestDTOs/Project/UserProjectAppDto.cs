namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record UserProjectAppDto(
    int OwnerId,
    string UserPassword,
    int ProjId
);