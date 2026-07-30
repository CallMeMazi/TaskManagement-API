namespace TaskManagement.Application.DTOs.RequestDTOs.User;
public record CreateUserAppDto(
    string MobileNumber,
    string Email,
    string Password,
    string FirstName,
    string LastName
);