namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public class DeleteOrgAppDto
{
    public int OrgId { get; set; }
    public int OwnerId { get; set; }
    public required string OwnerPassword { get; set; }
}
