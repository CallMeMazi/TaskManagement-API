using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Query.ValidateAccessToken;
public class ValidateAccessTokenHandler
    : IRequestHandler<ValidateAcceessTokenQuery, GeneralResult>
{
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public ValidateAccessTokenHandler(IAuthServiec authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ValidateAcceessTokenQuery request, CancellationToken ct)
    {
        var requestDto = _mapper.Map<ValidateUserTokenAppDto>(request);

        return _authService.ValidateAccessTokenAsync(requestDto, ct);
    }
}
