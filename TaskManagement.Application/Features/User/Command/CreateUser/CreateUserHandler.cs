using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.CreateUserCommand;

public class CreateUserHandler
    : IRequestHandler<CreateUserCommand, GeneralResult<UserTokenDto>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public CreateUserHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public Task<GeneralResult<UserTokenDto>> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<CreateUserAppDto>(request);

        return _userService.CreateUserAsync(dto, ct);
    }
}