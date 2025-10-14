namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public class ChangeActivityOrgAppDto
{
    public required string orgCode { get; set; }
    public int UserId { get; set; }
    public bool Activity { get; set; }
}
