using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensExceptCurrentByUserId;
public class RevokeAllTokensExceptCurrentByUserIdHandler
    : IRequestHandler<RevokeAllTokensExceptCurrentByUserIdCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public RevokeAllTokensExceptCurrentByUserIdHandler(IAuthServiec authService, IMapper mapper, IUnitOfWork uow)
    {
        _authService = authService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(RevokeAllTokensExceptCurrentByUserIdCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<RevokeUserTokenAppDto>(request);

        var revokeAllTokens = await _authService.RevokeAllTokensExceptCurrentByUserIdAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return revokeAllTokens;
    }
}
