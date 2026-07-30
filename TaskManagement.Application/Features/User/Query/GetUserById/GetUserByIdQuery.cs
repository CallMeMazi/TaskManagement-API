using MediatR;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Query.GetUserById;
public record GetUserByIdQuery(int UserId)
    : IRequest<GeneralResult<UserDetailsDto>>;
