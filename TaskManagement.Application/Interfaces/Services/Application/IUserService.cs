using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Main;
public interface IUserService
{
    Task<GeneralResult<UserDetailsDto>> GetUserByIdAsync(int id, CancellationToken ct);
    Task<GeneralResult<UserDetailsDto>> GetUserByMobileNumberAsync(string mobileNumber, CancellationToken ct);
    Task<GeneralResult<UserTokenDto>> CreateUserAsync(CreateUserAppDto command, CancellationToken ct);
    Task<GeneralResult> UpdateUserAsync(UpdateUserAppDto command, CancellationToken ct);
    Task<GeneralResult> SoftDeleteUserAsync(DeleteUserAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangePasswordUserAsync(ChangePasswordUserAppDto command, CancellationToken ct);
    Task<GeneralResult> IncreaseUserPointsAsync(int id, CancellationToken ct);
    Task<GeneralResult> DecreaseUserPointsAsync(int id, CancellationToken ct);
}
