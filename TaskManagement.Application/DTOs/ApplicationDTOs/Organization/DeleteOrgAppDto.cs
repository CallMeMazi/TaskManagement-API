namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public class DeleteOrgAppDto
{
    public int OrgId { get; set; }
    public int UserId { get; set; }
    public required string UserPassword { get; set; }
}
