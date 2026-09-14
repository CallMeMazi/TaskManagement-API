using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeTokenByDeviceId;
public class RevokeTokenByDeviceIdHandler
    : IRequestHandler<RevokeTokenByDeviceIdCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public RevokeTokenByDeviceIdHandler(IAuthServiec authService, IMapper mapper, IUnitOfWork uow)
    {
        _authService = authService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(RevokeTokenByDeviceIdCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<RevokeUserTokenAppDto>(request);

        var revokeTokenRes = await _authService.RevokeTokenByDeviceIdAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return revokeTokenRes;
    }
}
