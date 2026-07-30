using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.LoginUser;
public record LoginUserCommand(
    string MobileNumber,
    string Password,
    string DeviceId,
    string UserIp,
    string UserAgent
) : IRequest<GeneralResult<UserTokenDto>>;