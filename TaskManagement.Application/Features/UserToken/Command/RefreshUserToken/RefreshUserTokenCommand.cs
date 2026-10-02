using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;

namespace TaskManagement.Application.Features.UserToken.Command.RefreshUserToken;
public record RefreshUserTokenCommand(string RefreshToken, string DeviceId)
    : IRequest<UserTokenDto>;