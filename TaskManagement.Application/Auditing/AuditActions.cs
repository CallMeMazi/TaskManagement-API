namespace TaskManagement.Application.Auditing;

public static class AuditActions
{
    public const string OrganizationActivated = "Organization.Activated";
    public const string OrganizationDeactivated = "Organization.Deactivated";
    public const string OrganizationRoleChanged = "Organization.RoleChanged";

    public const string ProjectActivated = "Project.Activated";
    public const string ProjectDeactivated = "Project.Deactivated";
    public const string ProjectStatusChanged = "Project.StatusChanged";
    public const string ProjectRoleChanged = "Project.RoleChanged";

    public const string TaskCreated = "Task.Created";
    public const string TaskUpdated = "Task.Updated";
    public const string TaskDeleted = "Task.Deleted";
    public const string TaskAssigned = "Task.Assigned";
    public const string TaskUnassigned = "Task.Unassigned";
    public const string TaskStatusChanged = "Task.StatusChanged";
    public const string TaskStarted = "Task.Started";
    public const string TaskEnded = "Task.Ended";
    public const string TaskActivated = "Task.Activated";
    public const string TaskDeactivated = "Task.Deactivated";
}
