using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Common.Settings;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Services;

namespace TaskManagement.Application.Services.Application;

public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserDomainService _userDomainService;
    private readonly AppSettings _appSettings;
    private readonly ICommonService _commonService;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IUserDomainService userDomainService, ICommonService commonService
        , IMapper mapper, AppSettings appSettings)
    {
        _uow = unitOfWork;
        _userDomainService = userDomainService;
        _commonService = commonService;
        _mapper = mapper;
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
    public async Task<GeneralResult<int>> CreateUserAsync(CreateUserAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        // Check mobile number exist
        await _userDomainService.EnsureCanCreateUserAsync(command.MobileNumber, ct);

        var userPassHash = _commonService.Password.Hash(command.Password);
        var user = _mapper.Map<User>(command, opt => opt.Items["PasswordHash"] = userPassHash);

        await _uow.User.AddAsync(user, ct);
        await _uow.SaveAsync(ct);

        return GeneralResult<int>.Success(user.Id);
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

        _commonService.Password.VerifyAndCheck(user!.PasswordHash, command.Password, "رمز عبور اشتلاه است!");

        // Check user has org
        // Check user in org
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

        _commonService.Password.VerifyAndCheck(user!.PasswordHash, command.OldPassword, "رمز عبور اشتباه است!");

        user.ChangeUserPassword(_commonService.Password.Hash(command.NewPassword));

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