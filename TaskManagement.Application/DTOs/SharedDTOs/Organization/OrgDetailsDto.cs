namespace TaskManagement.Application.DTOs.SharedDTOs.Organization;
public class OrgDetailsDto
{
    public required string OrgName { get; set; }
    public required string SecondOrgName { get; set; }
    public required string OrgCode { get; set; }
    public required string OrgDescription { get; set; }
    public bool IsActive { get; set; }
    public byte MaxUser { get; set; }
    public DateTime CreateAt { get; set; }
}
