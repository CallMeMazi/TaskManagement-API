using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Main;
public interface IUserService
{
    Task<GeneralResult<UserDetailsDto>> GetUserByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken);
    Task<GeneralResult> CreateUserAsync(CreateUserAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> UpdateUserAsync(UpdateUserAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> SoftDeleteUserAsync(DeleteUserAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> ChangePasswordUserAsync(ChangePasswordUserAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> LoginUserAsync(LoginUserAppDto command, CancellationToken cancellationToken);
}
