﻿namespace TaskManagement.Application.DTOs.ApplicationDTOs.User;
public class CreateUserAppDto
{
    public required string MobileNumber { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
