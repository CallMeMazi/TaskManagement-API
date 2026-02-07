namespace TaskManagement.Domin.Interface.Services;
public interface IUserTokenDomainService
{
    Task EnsureCanLoginAsync(int userId, CancellationToken ct);
}
