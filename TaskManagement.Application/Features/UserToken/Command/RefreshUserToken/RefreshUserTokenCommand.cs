using MediatR;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RefreshToken;
public record RefreshUserTokenCommand(string RefreshToken, string DeviceId)
    : IRequest<GeneralResult<UserTokenDto>>;