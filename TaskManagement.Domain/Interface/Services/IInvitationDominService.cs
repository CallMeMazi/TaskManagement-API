namespace TaskManagement.Domain.Interface.Services;
public interface IInvitationDomainService
{
    Task EnsureCanGenerateInviteLinkAsync(long orgId, long orgOwnerId, long userId, CancellationToken ct);
}
