using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;

namespace TaskManagement.Application.Features.User.Query.GetUserById;
public class GetUserByIdHandler
    : IRequestHandler<GetUserByIdQuery, UserDetailsDto>
{
    private readonly IUserService _userService;

    public GetUserByIdHandler(IUserService userService)
    {
        _userService = userService;
    }

    public Task<UserDetailsDto> Handle(GetUserByIdQuery request, CancellationToken ct)
        => _userService.GetUserByIdAsync(request.UserId, ct);
}
