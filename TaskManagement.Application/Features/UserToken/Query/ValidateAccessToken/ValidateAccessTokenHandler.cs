using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Query.ValidateAccessToken;
public class ValidateAccessTokenHandler
    : IRequestHandler<ValidateAcceessTokenQuery, GeneralResult>
{
    private readonly IAuthServiec _authService;
    private readonly ICommonService _common;

    public ValidateAccessTokenHandler(IAuthServiec authService, ICommonService common)
    {
        _authService = authService;
        _common = common;
    }

    public Task<GeneralResult> Handle(ValidateAcceessTokenQuery request, CancellationToken ct)
    {
        var requestDto = _common.Mapper.Map<ValidateUserTokenAppDto>(request);

        return _authService.ValidateAccessTokenAsync(requestDto, ct);
    }
}
