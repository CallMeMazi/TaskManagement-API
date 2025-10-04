namespace TaskManagement.Application.DTOs.SharedDTOs.User;
public class UserDetailsDto
{
    public required string MobileNumber { get; set; }
    public required string Eemail { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public byte Point { get; set; }
    public DateTime CreateAt { get; set; }
}
