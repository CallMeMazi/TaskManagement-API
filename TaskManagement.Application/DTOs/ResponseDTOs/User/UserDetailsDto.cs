namespace TaskManagement.Application.DTOs.ResponseDTOs.User;
public record UserDetailsDto(
    string MobileNumber,
    string Email,
    string FirstName,
    string LastName,
    byte Points,
    DateTime CreatedAt
);