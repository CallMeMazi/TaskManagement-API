using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Main;
public interface IUserService
{
    Task<GeneralResult<UserDetailsDto>> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<GeneralResult<UserDetailsDto>> GetUserByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken);
    Task<GeneralResult<UserTokenDto>> CreateUserAsync(CreateUserAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> UpdateUserAsync(UpdateUserAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> SoftDeleteUserAsync(DeleteUserAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> ChangePasswordUserAsync(ChangePasswordUserAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> IncreaseUserPointsAsync(int id, CancellationToken cancellationToken);
    Task<GeneralResult> DecreaseUserPointsAsync(int id, CancellationToken cancellationToken);
}
