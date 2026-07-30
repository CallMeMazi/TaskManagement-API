using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;

namespace TaskManagement.Application.Interfaces.Services.Halper;
public interface IEventService
{
    // Auth service methods
    Task<UserTokenDto> PublishRegisterUserEventAsync(RegisterUserTokenAppDto command, CancellationToken ct);
    Task PublishRevokeAllTokensExceptCurrentByUserIdEventAsync(RevokeUserTokenAppDto command, bool isSaved, CancellationToken ct);

    // Org service methods
    Task PublishAddUserToOrgEventAsync(AddUserOrgAppDto command, CancellationToken ct);

    // TaskInfo service methods
    Task PublishCreateTaskInfoAsync(CreateTaskInfoAppDto command, CancellationToken ct);
}
