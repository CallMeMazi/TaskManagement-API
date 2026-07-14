namespace TaskManagement.Application.DTOs.SharedDTOs.User;
public record UserDetailsDto(
    string MobileNumber,
    string Email,
    string FirstName,
    string LastName,
    byte Point,
    DateTime CreateAt
);