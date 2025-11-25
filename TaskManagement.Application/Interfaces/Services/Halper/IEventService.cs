using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;

namespace TaskManagement.Application.Interfaces.Services.Halper;
public interface IEventService
{
    // Auth service methods
    Task<UserTokenDto> PublishRegisterUserEventAsync(RegisterUserTokenAppDto command, CancellationToken ct);
    Task PublishRevokeAllTokensExceptCurrentByUserIdEventAsync(RevokeUserTokenAppDto command, bool isSaved, CancellationToken ct);
}
