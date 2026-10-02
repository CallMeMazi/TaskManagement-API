using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;

namespace TaskManagement.Application.Features.User.Query.GetUserByMobileNumber;
public class GetUserByMobileNumberHandler
    : IRequestHandler<GetUserByMobileNumberQuery, UserDetailsDto>
{
    private readonly IUserService _userService;

    public GetUserByMobileNumberHandler(IUserService userService)
    {
        _userService = userService;
    }

    public Task<UserDetailsDto> Handle(GetUserByMobileNumberQuery request, CancellationToken ct)
        => _userService.GetUserByMobileNumberAsync(request.MobileNumber, ct);
}
