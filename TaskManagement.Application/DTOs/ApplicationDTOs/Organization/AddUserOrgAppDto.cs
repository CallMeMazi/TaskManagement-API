namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public class AddUserOrgAppDto
{
    public int OrgOwnerId { get; set; }
    public required string UserMobileNumber { get; set; }
    public int OrgId { get; set; }
}
