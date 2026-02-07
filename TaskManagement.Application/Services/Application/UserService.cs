using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Common.Settings;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Interface.Services;

namespace TaskManagement.Application.Services.Application;
public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserDomainService _userDomainService;
    private readonly AppSettings _appSettings;
    private readonly ICommonService _commonService;
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;


    public UserService(IUnitOfWork unitOfWork, IUserDomainService userDomainService, ICommonService commonService
        , IMapper mapper, IEventService eventService, AppSettings appSettings)
    {
        _uow = unitOfWork;
        _userDomainService = userDomainService;
        _commonService = commonService;
        _mapper = mapper;
        _eventService = eventService;
        _appSettings = appSettings;
    }


    // Query methods
    public async Task<GeneralResult<UserDetailsDto>> GetUserByIdAsync(int id, CancellationToken ct)
    {
        var user = await _uow.User.GetByIdAsync(id, false, ct);

        if (user.IsNullParameter())
            throw new NotFoundException("کاربری با این آیدی وجود ندارد!");

        var userDto = _mapper.Map<UserDetailsDto>(user);

        return GeneralResult<UserDetailsDto>.Success(userDto);
    }
    public async Task<GeneralResult<UserDetailsDto>> GetUserByMobileNumberAsync(string mobileNumber, CancellationToken ct)
    {
        var user = await _uow.User.GetByFilterAsync(u => u.MobileNumber == mobileNumber, false, ct);

        if (user == null)
            throw new NotFoundException("کاربری با این شماره موبایل وجود ندارد!");

        var userDto = _mapper.Map<UserDetailsDto>(user);

        return GeneralResult<UserDetailsDto>.Success(userDto);
    }

    // Command methods
    public async Task<GeneralResult<UserTokenDto>> CreateUserAsync(CreateUserAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        await _userDomainService.EnsureCanCreateUserAsync(command.MobileNumber, ct);

        command.Password = _commonService.Password.Hash(command.Password);

        var user = _mapper.Map<User>(command);

        await _uow.User.AddAsync(user, ct);
        await _uow.SaveAsync(ct);

        // Generate User tokens(regester) after creation (Event)
        var tokens = await _eventService.PublishRegisterUserEventAsync(
            new RegisterUserTokenAppDto()
            {
                user = user,
                DeviceId = command.DeviceId,
                UserAgent = command.UserAgent,
                UserIp = command.UserIp,
            },
            ct
        );

        return GeneralResult<UserTokenDto>.Success(tokens);
    }
    public async Task<GeneralResult> UpdateUserAsync(UpdateUserAppDto command, CancellationToken ct)
    {
        var user = await _uow.User.GetByIdAsync(command.UserId, true, ct);
        if (user.IsNullParameter())
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(UpdateUserAsync)} method!");

        user!.UpdateUser(command.Email, command.FirstName, command.LastName);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteUserAsync(DeleteUserAppDto command, CancellationToken ct)
    {
        // This method use SP (Stored Procedure)

        var user = await _uow.User.GetByIdAsync(command.UserId, false, ct);
        if (user.IsNullParameter())
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(SoftDeleteUserAsync)} method!");

        if (_commonService.Password.Verify(user!.PasswordHash, command.Password))
            throw new BadRequestException("رمز عبور اشتلاه است!");

        await _userDomainService.EnsureCanDeleteUserAsync(command.UserId, ct);

        // Delete User (SP)
        // Delete all UserTokens By UserId (SP)
        // Delete All Orgs By UserId (SP)
        // Delete All OrgMemberships By OrgId (SP)
        // Delete All Projects By OrgId (SP)
        // Delete All ProjectMemberships By ProjectId (SP)
        // Delete All Tasks By ProjectId (SP)
        // Delete All TaskAssignments By ProjectId (SP)
        // Delete All TaslInfos By TaskId (SP)
        await _uow.User.SoftDeleteUserSpAsync(user.Id, ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangePasswordUserAsync(ChangePasswordUserAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        var user = await _uow.User.GetByIdAsync(command.UserId, true, ct);
        if (user.IsNullParameter())
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(ChangePasswordUserAsync)} method!");

        if (!_commonService.Password.Verify(user!.PasswordHash, command.OldPassword))
            throw new BadRequestException("رمز عبور اشتباه است!");

        user.ChangeUserPassword(_commonService.Password.Hash(command.NewPassword));

        // revoke all User tokens except current (Event)
        await _eventService.PublishRevokeAllTokensExceptCurrentByUserIdEventAsync(
            new RevokeUserTokenAppDto()
            {
                UserId = user.Id,
                Deviceid = command.DeviceId
            },
            false,
            ct
        );

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> IncreaseUserPointsAsync(int id, CancellationToken ct)
    {
        var user = await _uow.User.GetByIdAsync(id, true, ct);
        if (user.IsNullParameter())
            throw new Exception($"user by {id} ID was not found. in {nameof(IncreaseUserPointsAsync)} method!");

        user!.IncreaseOrDecreasePoints(_appSettings.UserSetting.PositiveUserPoints);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> DecreaseUserPointsAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _uow.User.GetByIdAsync(id, true, cancellationToken);
        if (user.IsNullParameter())
            throw new Exception($"user by {id} ID was not found. in {nameof(DecreaseUserPointsAsync)} method!");

        user!.IncreaseOrDecreasePoints(_appSettings.UserSetting.NegativeUserPoints);
        await _uow.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
}
