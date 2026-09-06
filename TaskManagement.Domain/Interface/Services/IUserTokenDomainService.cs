namespace TaskManagement.Domain.Interface.Services;
public interface IUserTokenDomainService
{
    Task EnsureCanLoginAsync(long userId, CancellationToken ct);
}
