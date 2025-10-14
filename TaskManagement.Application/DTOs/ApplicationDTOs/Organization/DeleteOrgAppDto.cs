namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public class DeleteOrgAppDto
{
    public required string OrgCode { get; set; }
    public required int UserId { get; set; }
    public required string UserPassword { get; set; }
}
