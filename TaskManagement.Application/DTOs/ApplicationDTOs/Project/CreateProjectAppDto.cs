namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public class CreateProjectAppDto
{
    public required string ProjName { get; set; }
    public required string ProjDescription { get; set; }
    public Guid OrgId { get; set; }
    public Guid CreatorId { get; set; }
    public byte MaxUser { get; set; }
    public byte MaxTask { get; set; }
}
