using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;

namespace TaskManagement.Application.Features.UserToken.Command.LoginUser;
public record LoginUserCommand(
    string MobileNumber,
    string Password,
    string DeviceId,
    string UserIp,
    string UserAgent
) : IRequest<UserTokenDto>;