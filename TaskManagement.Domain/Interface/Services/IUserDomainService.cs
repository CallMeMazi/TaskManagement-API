namespace TaskManagement.Domain.Interface.Services;
public interface IUserDomainService
{
    Task EnsureCanCreateUserAsync(string mobileNumber, CancellationToken ct);
    Task EnsureCanDeleteUserAsync(int userId, CancellationToken ct);
}
