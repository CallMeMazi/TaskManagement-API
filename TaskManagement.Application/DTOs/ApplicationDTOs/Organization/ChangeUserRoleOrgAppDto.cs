namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public class ChangeUserRoleOrgAppDto
{
    public int OrgOwnerId { get; set; }
    public int OrgId { get; set; }
    public int UserId { get; set; }
}
