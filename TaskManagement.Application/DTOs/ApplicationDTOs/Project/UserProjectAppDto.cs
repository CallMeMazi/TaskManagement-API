namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public class UserProjectAppDto
{
    public int OwnerId { get; set; }
    public required string UserPassword { get; set; }
    public int ProjId { get; set; }
}
