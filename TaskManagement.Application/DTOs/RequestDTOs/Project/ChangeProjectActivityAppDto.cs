namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record ChangeProjectActivityAppDto(
    int OwnerId,
    int ProjId,
    string UserPassword,
    bool Activity
);