using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Main;
public interface IAuthServiec
{
    Task<GeneralResult<List<UserTokenDetailsDto>>> GetUserActiveTokensAsync(int userId, CancellationToken ct);
    Task<GeneralResult<UserTokenDto>> LoginUserAsync(LoginUserAppDto command, CancellationToken ct);
    Task<GeneralResult> LogoutUserAsync(LogoutUserAppDto command, CancellationToken ct);
    Task<GeneralResult<UserTokenDto>> RefreshTokenAsync(RefreshUserTokenAppDto command, CancellationToken ct);
    Task<GeneralResult<UserTokenDto>> RegisterUserAsync(RegisterUserTokenAppDto command, CancellationToken ct);
    Task<GeneralResult> RevokeAllTokensByUserIdAsync(int userId, bool isSaved, CancellationToken ct);
    Task<GeneralResult> RevokeAllTokensExceptCurrentByUserIdAsync(RevokeUserTokenAppDto command, bool isSaved, CancellationToken ct);
    Task<GeneralResult> RevokeTokenByUserIdAndIpAsync(RevokeUserTokenAppDto command, CancellationToken ct);
    Task<GeneralResult> ValidateAccessTokenAsync(validateUserTokenAppDto query, CancellationToken ct);
}
