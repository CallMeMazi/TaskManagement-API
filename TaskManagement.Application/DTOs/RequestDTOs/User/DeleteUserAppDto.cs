namespace TaskManagement.Application.DTOs.RequestDTOs.User;
public record DeleteUserAppDto(
    int UserId,
    string Password
);