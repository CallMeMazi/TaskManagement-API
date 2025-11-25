namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public class ChangeActivityOrgAppDto
{
    public int OrgId { get; set; }
    public int OwnerId { get; set; }
    public required string OwnerPassword { get; set; }
    public bool Activity { get; set; }
}
