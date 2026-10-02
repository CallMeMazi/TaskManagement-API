using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;

namespace TaskManagement.Application.Features.UserToken.Query.ValidateAccessToken;
public class ValidateAccessTokenHandler
    : IRequestHandler<ValidateAcceessTokenQuery>
{
    private readonly IAuthServiec _authService;
    private readonly ICommonService _common;

    public ValidateAccessTokenHandler(IAuthServiec authService, ICommonService common)
    {
        _authService = authService;
        _common = common;
    }

    public async System.Threading.Tasks.Task Handle(ValidateAcceessTokenQuery request, CancellationToken ct)
    {
        var requestDto = _common.Mapper.Map<ValidateUserTokenAppDto>(request);

        await _authService.ValidateAccessTokenAsync(requestDto, ct);
    }
}
