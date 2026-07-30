using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.CreateUser;

public record CreateUserCommand(
    string MobileNumber,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string DeviceId,
    string UserIp,
    string UserAgent
) : IRequest<GeneralResult<UserTokenDto>>;