using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.DeleteUser;

public class DeleteUserHandler
    : IRequestHandler<DeleteUserCommand, GeneralResult>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public DeleteUserHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<DeleteUserAppDto>(request);

        return _userService.SoftDeleteUserAsync(dto, ct);
    }
}