using MediatR;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Query.GetUserById;
public class GetUserByIdHandler
    : IRequestHandler<GetUserByIdQuery, GeneralResult<UserDetailsDto>>
{
    private readonly IUserService _userService;

    public GetUserByIdHandler(IUserService userService)
    {
        _userService = userService;
    }

    public Task<GeneralResult<UserDetailsDto>> Handle(GetUserByIdQuery request, CancellationToken ct)
        => _userService.GetUserByIdAsync(request.UserId, ct);
}
