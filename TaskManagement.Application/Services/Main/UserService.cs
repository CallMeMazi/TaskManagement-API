using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Main;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Common.Settings;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Enums.Roles;

namespace TaskManagement.Application.Services.Main;
public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;
    private readonly AppSettings _appSettings;
    private readonly ICommonService _commonService;
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;


    public UserService(IUnitOfWork unitOfWork, ICommonService commonService
        , IMapper mapper, IEventService eventService, AppSettings appSettings)
    {
        _uow = unitOfWork;
        _commonService = commonService;
        _mapper = mapper;
        _eventService = eventService;
        _appSettings = appSettings;
    }


    // Query methods
    public async Task<GeneralResult<UserDetailsDto>> GetUserByIdAsync(int id, CancellationToken ct)
    {
        if (id.IsNullParameter() || id <= 0)
            throw new BadRequestException("آیدی نامعتبر است!");

        var user = await _uow.User.GetDtoByIdAsync(id, ct);

        if (user.IsNullParameter())
            throw new NotFoundException("کاربری با این آیدی وجود ندارد!");

        return GeneralResult<UserDetailsDto>.Success(user)!;
    }
    public async Task<GeneralResult<UserDetailsDto>> GetUserByMobileNumberAsync(string mobileNumber, CancellationToken ct)
    {
        if (mobileNumber.IsNullParameter())
            throw new BadRequestException("شماره موبایل خالی است!");

        var user = await _uow.User.GetDtoByFilterAsync(u => u.MobileNumber == mobileNumber, ct);

        if (user == null)
            throw new NotFoundException("کاربری با این شماره موبایل وجود ندارد!");

        return GeneralResult<UserDetailsDto>.Success(user)!;
    }

    // Command methods
    public async Task<GeneralResult<UserTokenDto>> CreateUserAsync(CreateUserAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        if (await _uow.User.IsEntityExistByFilterAsync(u => u.MobileNumber == command.MobileNumber, ct))
            throw new BadRequestException("کاربری با این شماره موبایل وجود دارد!");

        command.Password = _commonService.Password.Hash(command.Password);

        var user = _mapper.Map<User>(command);

        await _uow.User.AddAsync(user, ct);
        await _uow.SaveAsync(ct);

        // Generate User tokens(regester) after creation (Event)
        var tokens = await _eventService.PublishRegisterUserEventAsync(
            new RegisterUserTokenAppDto()
            {
                user = user,
                UserAgent = command.UserAgent,
                UserIp = command.UserIp,
            },
            ct
        );

        return GeneralResult<UserTokenDto>.Success(tokens)!;
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

        var verifyResult = await VerifyUserForDeleteAsync(user.Id, ct);
        if (!verifyResult.IsSuccess)
            throw new BadRequestException(verifyResult.Message);

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

        command.NewPassword = _commonService.Password.Hash(command.NewPassword);

        user.ChangeUserPassword(command.NewPassword);

        // revoke all User tokens except current (Event)
        await _eventService.PublishRevokeAllTokensExceptCurrentByUserIdEventAsync(
            new RevokeUserTokenAppDto()
            {
                UserId = user.Id,
                UserIp = command.UserIp,
                UserAgent = command.UserAgent
            },
            false,
            ct
        );

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> IncreaseUserPointsAsync(int id, CancellationToken ct)
    {
        if (id.IsNullParameter() || id <= 0)
            throw new BadRequestException("آیدی نامعتبر است!");

        var user = await _uow.User.GetByIdAsync(id, true, ct);
        if (user.IsNullParameter())
            throw new NotFoundException("کاربری با این آیدی وجود ندارد!");

        user!.IncreaseOrDecreasePoints(_appSettings.UserSetting.PositiveUserPoints);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> DecreaseUserPointsAsync(int id, CancellationToken cancellationToken)
    {
        if (id.IsNullParameter() || id <= 0)
            throw new BadRequestException("آیدی نامعتبر است!");

        var user = await _uow.User.GetByIdAsync(id, true, cancellationToken);
        if (user.IsNullParameter())
            throw new NotFoundException("کاربری با این آیدی وجود ندارد!");

        user!.IncreaseOrDecreasePoints(_appSettings.UserSetting.NegativeUserPoints);
        await _uow.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }

    private async Task<GeneralResult> VerifyUserForDeleteAsync(int userId, CancellationToken ct)
    {
        if (await _uow.Organization.IsEntityExistByFilterAsync(o => o.OwnerId == userId && o.IsActive, ct))
            return GeneralResult.Failure("شما هنوز سازمان فعال دارید، اول سازمان های خود را غیرفعال کنید!");

        var isUserInOtherOrgs = await _uow.OrganizationMemberShip.IsEntityExistByFilterAsync(om =>
            om.UserId == userId
            && om.Role != OrganizationRoles.Owner,
            ct
        );
        if (isUserInOtherOrgs)
            return GeneralResult.Failure("شما در سازمان های دیگری عضو هستید، ابتدا از تمام سازمان هایی که عضو هستید خارج شوید!");

        return GeneralResult.Success();
    }
}
