using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Main;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Common.Settings;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Services.Main;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppSettings _appSettings;
    private readonly ICommonService _commonService;
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;


    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, ICommonService commonService
        , IMapper mapper, IEventService eventService, AppSettings appSettings)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _commonService = commonService;
        _mapper = mapper;
        _eventService = eventService;
        _appSettings = appSettings;
    }


    // query services
    public async Task<GeneralResult<UserDetailsDto>> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id.IsNullParameter() || id <= 0)
            throw new BadRequestException("آیدی نامعتبر است!");

        var user = await _userRepository.GetUserDtoByIdAsync(id, cancellationToken);

        if (user.IsNullParameter())
            throw new NotFoundException("کاربری با این آیدی وجود ندارد!");

        return GeneralResult<UserDetailsDto>.Success(user)!;
    }
    public async Task<GeneralResult<UserDetailsDto>> GetUserByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken)
    {
        if (mobileNumber.IsNullParameter())
            throw new BadRequestException("شماره موبایل خالی است!");

        var user = await _userRepository.GetUserDtoByFilterAsync(u => u.MobileNumber == mobileNumber, cancellationToken);

        if (user == null)
            throw new NotFoundException("کاربری با این شماره موبایل وجود ندارد!");

        return GeneralResult<UserDetailsDto>.Success(user)!;
    }

    // command services
    public async Task<GeneralResult<UserTokenDto>> CreateUserAsync(CreateUserAppDto command, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        if (await _userRepository.IsUserExistByFilterAsync(u => u.MobileNumber == command.MobileNumber, cancellationToken))
            throw new BadRequestException("کاربری با این شماره موبایل وجود دارد!");

        command.Password = _commonService.Password.Hash(command.Password);

        var user = _mapper.Map<User>(command);

        await _userRepository.AddUserAsync(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        // generate User tokens(regester) after creation (Event)
        var tokens = await _eventService.PublishRegisterUserEventAsync(
            new RegisterUserTokenAppDto()
            {
                user = user,
                UserAgent = command.UserAgent,
                UserIp = command.UserIp,
            },
            cancellationToken
        );

        return GeneralResult<UserTokenDto>.Success(tokens)!;
    }
    public async Task<GeneralResult> UpdateUserAsync(UpdateUserAppDto command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(command.UserId, true, cancellationToken);
        if (user.IsNullParameter())
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(UpdateUserAsync)} method!");

        user!.UpdateUser(command.Email, command.FirstName, command.LastName);
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteUserAsync(DeleteUserAppDto command, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        var user = await _userRepository.GetUserByIdAsync(command.UserId, true, cancellationToken);
        if (user.IsNullParameter())
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(SoftDeleteUserAsync)} method!");

        user!.SoftDelete();
        user.ChangeSecurityStamp();

        // revoke all User tokens (Event)
        await _eventService.PublishRevokeAllTokensEventAsync(command.UserId, false, cancellationToken);
        // inactive User Org (Event)
        // remove User from other Orgs (Event)
        // remove user from Projects (Event)
        // inactive User Tasks (Event)
        // and ...

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangePasswordUserAsync(ChangePasswordUserAppDto command, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        var user = await _userRepository.GetUserByIdAsync(command.UserId, true, cancellationToken);
        if (user.IsNullParameter())
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(ChangePasswordUserAsync)} method!");

        if (!_commonService.Password.Verify(user!.PasswordHash, command.OldPassword))
            throw new BadRequestException("رمز عبور اشتباه است!");

        command.NewPassword = _commonService.Password.Hash(command.NewPassword);

        user.ChangeUserPassword(command.NewPassword);

        // revoke all User tokens except current (Event)
        await _eventService.PublishRevokeAllTokensExceptCurrentEventAsync(
            new RevokeUserTokenAppDto()
            {
                UserId = user.Id,
                UserIp = command.UserIp,
                UserAgent = command.UserAgent
            },
            false,
            cancellationToken
        );

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> IncreaseUserPointsAsync(int id, CancellationToken cancellationToken)
    {
        if (id.IsNullParameter() || id <= 0)
            throw new BadRequestException("آیدی نامعتبر است!");

        var user = await _userRepository.GetUserByIdAsync(id, true, cancellationToken);
        if (user.IsNullParameter())
            throw new NotFoundException("کاربری با این آیدی وجود ندارد!");

        user!.IncreaseOrDecreasePoints(_appSettings.UserSetting.PositiveUserPoints);
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> DecreaseUserPointsAsync(int id, CancellationToken cancellationToken)
    {
        if (id.IsNullParameter() || id <= 0)
            throw new BadRequestException("آیدی نامعتبر است!");

        var user = await _userRepository.GetUserByIdAsync(id, true, cancellationToken);
        if (user.IsNullParameter())
            throw new NotFoundException("کاربری با این آیدی وجود ندارد!");

        user!.IncreaseOrDecreasePoints(_appSettings.UserSetting.NegativeUserPoints);
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
}
