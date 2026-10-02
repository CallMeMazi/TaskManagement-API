using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.User;

namespace TaskManagement.Application.Features.User.Query.GetUserById;
public record GetUserByIdQuery(long UserId)
    : IRequest<UserDetailsDto>;
