namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public class CreateOrgAppDto
{
    public required string OrgName { get; set; }
    public required string SecondOrgName { get; set; }
    public required string OrgDescription { get; set; }
    public int OwnerId { get; set; }
    public byte MaxUser { get; set; }
}
