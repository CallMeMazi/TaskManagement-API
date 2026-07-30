namespace TaskManagement.Application.DTOs.ApplicationDTOs.User;
public record CreateUserAppDto(
    string MobileNumber,
    string Email,
    string Password,
    string FirstName,
    string LastName
);