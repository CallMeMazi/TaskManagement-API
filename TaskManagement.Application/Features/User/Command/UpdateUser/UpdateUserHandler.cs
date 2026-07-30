using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.UpdateUserCommand;

public class UpdateUserHandler
    : IRequestHandler<UpdateUserCommand, GeneralResult>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UpdateUserHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UpdateUserAppDto>(request);

        return _userService.UpdateUserAsync(dto, ct);
    }
}