using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;

namespace TaskManagement.Application.Features.UserToken.Query.GetUserActiveTokens;
public class GetUserActiveTokensHandler
    : IRequestHandler<GetUserActiveTokensQuery, List<UserTokenDetailsDto>>
{
    private readonly IAuthServiec _authService;

    public GetUserActiveTokensHandler(IAuthServiec authService)
    {
        _authService = authService;
    }

    public Task<List<UserTokenDetailsDto>> Handle(GetUserActiveTokensQuery request, CancellationToken ct)
        => _authService.GetUserActiveTokensAsync(request.UserId, ct);
}
