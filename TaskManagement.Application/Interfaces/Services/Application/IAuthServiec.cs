using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface IAuthServiec
{
    Task<List<UserTokenDetailsDto>> GetUserActiveTokensAsync(long userId, CancellationToken ct);
    Task<UserTokenDto> LoginUserAsync(LoginUserAppDto command, CancellationToken ct);
    Task LogoutUserAsync(LogoutUserAppDto command, CancellationToken ct);
    Task<UserTokenDto> RefreshTokenAsync(RefreshUserTokenAppDto command, CancellationToken ct);
    Task<UserTokenDto> RegisterUserAsync(RegisterUserTokenAppDto command, CancellationToken ct);
    Task RevokeAllTokensByUserIdAsync(long userId, CancellationToken ct);
    Task RevokeAllTokensExceptCurrentByUserIdAsync(RevokeUserTokenAppDto command, CancellationToken ct);
    Task RevokeTokenByDeviceIdAsync(RevokeUserTokenAppDto command, CancellationToken ct);
    Task ValidateAccessTokenAsync(ValidateUserTokenAppDto query, CancellationToken ct);
}
