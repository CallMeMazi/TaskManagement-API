using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.ChangePasswordUser;

public record ChangePasswordUserHandler
    : IRequestHandler<ChangePasswordUserCommand, GeneralResult>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public ChangePasswordUserHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangePasswordUserCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangePasswordUserAppDto>(request);

        return _userService.ChangePasswordUserAsync(dto, ct);
    }
}