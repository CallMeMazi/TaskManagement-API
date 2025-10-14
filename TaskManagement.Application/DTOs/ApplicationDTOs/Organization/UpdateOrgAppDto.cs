namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public class UpdateOrgAppDto
{
    public int UserId { get; set; }
    public required string OrgCode { get; set; }
    public required string OrgName { get; set; }
    public required string SecondOrgName { get; set; }
    public required string OrgDescription { get; set; }
}
