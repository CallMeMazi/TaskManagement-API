using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface IAuthServiec
{
    Task<GeneralResult<List<UserTokenDetailsDto>>> GetUserActiveTokensAsync(long userId, CancellationToken ct);
    Task<GeneralResult<UserTokenDto>> LoginUserAsync(LoginUserAppDto command, CancellationToken ct);
    Task<GeneralResult> LogoutUserAsync(LogoutUserAppDto command, CancellationToken ct);
    Task<GeneralResult<UserTokenDto>> RefreshTokenAsync(RefreshUserTokenAppDto command, CancellationToken ct);
    Task<GeneralResult<UserTokenDto>> RegisterUserAsync(RegisterUserTokenAppDto command, CancellationToken ct);
    Task<GeneralResult> RevokeAllTokensByUserIdAsync(long userId, CancellationToken ct);
    Task<GeneralResult> RevokeAllTokensExceptCurrentByUserIdAsync(RevokeUserTokenAppDto command, CancellationToken ct);
    Task<GeneralResult> RevokeTokenByDeviceIdAsync(RevokeUserTokenAppDto command, CancellationToken ct);
    Task<GeneralResult> ValidateAccessTokenAsync(ValidateUserTokenAppDto query, CancellationToken ct);
}
