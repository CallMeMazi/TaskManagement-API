namespace TaskManagement.Domain.Interface.Services;
public interface IUserTokenDomainService
{
    Task EnsureCanLoginAsync(int userId, CancellationToken ct);
}
