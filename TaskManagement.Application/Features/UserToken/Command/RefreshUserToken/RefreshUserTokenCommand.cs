using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RefreshUserToken;
public record RefreshUserTokenCommand(string RefreshToken, string DeviceId)
    : IRequest<GeneralResult<UserTokenDto>>;