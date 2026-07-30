namespace TaskManagement.Application.DTOs.ApplicationDTOs.User;
public record DeleteUserAppDto(
    int UserId,
    string Password
);