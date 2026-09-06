namespace TaskManagement.Application.DTOs.RequestDTOs.Project;
public record ChangeProjectActivityAppDto(
    long OwnerId,
    long ProjId,
    string UserPassword,
    bool Activity
);