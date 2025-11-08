using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Main;
public interface IAuthServiec
{
    Task<GeneralResult<List<UserTokenDetails>>> GetUserActiveTokensAsync(int userId, CancellationToken cancellationToken);
    Task<GeneralResult<UserTokenDto>> LoginUserAsync(LoginUserAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> LogoutUserAsync(LogoutUserAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult<UserTokenDto>> RefreshTokenAsync(RefreshUserTokenAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult<UserTokenDto>> RegisterUserAsync(RegisterUserTokenAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> RevokeAllTokensByUserIdAsync(int userId, bool isSaved, CancellationToken cancellationToken);
    Task<GeneralResult> RevokeAllTokensExceptCurrentAsync(RevokeUserTokenAppDto command, bool isSaved, CancellationToken cancellationToken);
    Task<GeneralResult> RevokeTokenByUserIdAndIpAsync(RevokeUserTokenAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> ValidateAccessTokenAsync(validateUserTokenAppDto command, CancellationToken cancellationToken);
}
