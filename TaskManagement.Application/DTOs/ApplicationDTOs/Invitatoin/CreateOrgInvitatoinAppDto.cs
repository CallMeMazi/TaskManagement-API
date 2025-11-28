namespace TaskManagement.Application.DTOs.ApplicationDTOs.Invitatoin;
public class CreateOrgInvitatoinAppDto
{
    public int OrgId { get; set; }
    public int OrgOwnerId { get; set; }
    public required string UserMobileNumber { get; set; }
}
