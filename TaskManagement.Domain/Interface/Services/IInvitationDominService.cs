namespace TaskManagement.Domain.Interface.Services;
public interface IInvitationDomainService
{
    Task EnsureCanGenerateInviteLinkAsync(int orgId, int orgOwnerId, int userId, CancellationToken ct);
}
