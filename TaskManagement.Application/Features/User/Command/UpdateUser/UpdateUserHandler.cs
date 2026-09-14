using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.UpdateUser;

public class UpdateUserHandler
    : IRequestHandler<UpdateUserCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UpdateUserHandler(IUserService userService, IMapper mapper, IUnitOfWork uow)
    {
        _userService = userService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UpdateUserAppDto>(request);

        var updateUserRes = await _userService.UpdateUserAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return updateUserRes;
    }
}