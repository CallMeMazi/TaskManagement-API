namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public record ChangeProjectActivityAppDto(
    int OwnerId,
    int ProjId,
    string UserPassword,
    bool Activity
);