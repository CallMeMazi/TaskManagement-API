using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.User;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Query.GetUserById;
public record GetUserByIdQuery(long UserId)
    : IRequest<GeneralResult<UserDetailsDto>>;
