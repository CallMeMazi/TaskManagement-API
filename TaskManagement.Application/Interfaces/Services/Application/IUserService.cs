using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.DTOs.ResponseDTOs.User;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface IUserService
{
    Task<UserDetailsDto> GetUserByIdAsync(long id, CancellationToken ct);
    Task<UserDetailsDto> GetUserByMobileNumberAsync(string mobileNumber, CancellationToken ct);
    Task<long> CreateUserAsync(CreateUserAppDto command, CancellationToken ct);
    Task UpdateUserAsync(UpdateUserAppDto command, CancellationToken ct);
    Task SoftDeleteUserAsync(DeleteUserAppDto command, CancellationToken ct);
    Task ChangePasswordUserAsync(ChangePasswordUserAppDto command, CancellationToken ct);
    Task IncreaseUserPointsAsync(long id, CancellationToken ct);
    Task DecreaseUserPointsAsync(long id, CancellationToken ct);
}
