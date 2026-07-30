using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Query.GetUserActiveTokens;
public class GetUserActiveTokensHandler
    : IRequestHandler<GetUserActiveTokensQuery, GeneralResult<List<UserTokenDetailsDto>>>
{
    private readonly IAuthServiec _authService;

    public GetUserActiveTokensHandler(IAuthServiec authService)
    {
        _authService = authService;
    }

    public Task<GeneralResult<List<UserTokenDetailsDto>>> Handle(GetUserActiveTokensQuery request, CancellationToken ct)
    {
        return _authService.GetUserActiveTokensAsync(request.UserId, ct);
    }
}
