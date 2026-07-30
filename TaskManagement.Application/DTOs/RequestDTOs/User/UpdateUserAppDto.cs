namespace TaskManagement.Application.DTOs.ApplicationDTOs.User;
public record UpdateUserAppDto(
    int UserId,
    string Email,
    string FirstName,
    string LastName
);