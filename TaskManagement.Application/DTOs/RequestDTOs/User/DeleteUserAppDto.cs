namespace TaskManagement.Application.DTOs.RequestDTOs.User;
public record DeleteUserAppDto(
    long UserId,
    string Password
);