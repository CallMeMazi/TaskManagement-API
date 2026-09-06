namespace TaskManagement.Application.DTOs.RequestDTOs.User;
public record UpdateUserAppDto(
    long UserId,
    string Email,
    string FirstName,
    string LastName
);