using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.DTOs.ResponseDTOs.User;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface IUserService
{
    Task<GeneralResult<UserDetailsDto>> GetUserByIdAsync(long id, CancellationToken ct);
    Task<GeneralResult<UserDetailsDto>> GetUserByMobileNumberAsync(string mobileNumber, CancellationToken ct);
    Task<GeneralResult<long>> CreateUserAsync(CreateUserAppDto command, CancellationToken ct);
    Task<GeneralResult> UpdateUserAsync(UpdateUserAppDto command, CancellationToken ct);
    Task<GeneralResult> SoftDeleteUserAsync(DeleteUserAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangePasswordUserAsync(ChangePasswordUserAppDto command, CancellationToken ct);
    Task<GeneralResult> IncreaseUserPointsAsync(long id, CancellationToken ct);
    Task<GeneralResult> DecreaseUserPointsAsync(long id, CancellationToken ct);
}
