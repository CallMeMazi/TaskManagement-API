using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensExceptCurrentByUserId;
public class RevokeAllTokensExceptCurrentByUserIdHandler
    : IRequestHandler<RevokeAllTokensExceptCurrentByUserIdCommand, GeneralResult>
{
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public RevokeAllTokensExceptCurrentByUserIdHandler(IAuthServiec authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(RevokeAllTokensExceptCurrentByUserIdCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<RevokeUserTokenAppDto>(request);

        return _authService.RevokeAllTokensExceptCurrentByUserIdAsync(dto, true, ct);
    }
}
