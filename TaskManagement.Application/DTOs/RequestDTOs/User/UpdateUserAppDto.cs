namespace TaskManagement.Application.DTOs.RequestDTOs.User;
public record UpdateUserAppDto(
    int UserId,
    string Email,
    string FirstName,
    string LastName
);