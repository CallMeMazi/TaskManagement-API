namespace TaskManagement.Domin.Interface.Services;
public interface IInvitationDominService
{
    Task EnsureCanGenerateInviteLinkAsync(int orgId, int orgOwnerId, int userId, CancellationToken ct);
}
