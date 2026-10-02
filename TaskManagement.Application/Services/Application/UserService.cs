using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.DTOs.ResponseDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Application.Utilities.Exceptions;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Services;

namespace TaskManagement.Application.Services.Application;

public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserDomainService _userDomainService;
    private readonly ICommonService _common;

    public UserService(IUnitOfWork unitOfWork, IUserDomainService userDomainService, ICommonService commonService)
    {
        _uow = unitOfWork;
        _userDomainService = userDomainService;
        _common = commonService;
    }

    // Query methods
    public async Task<GeneralResult<UserDetailsDto>> GetUserByIdAsync(long id, CancellationToken ct)
    {
        var user = await _uow.User.GetByIdAsync(id, false, ct);
        if (user.IsNullParameter())
            throw new NotFoundException("کاربری با این آیدی وجود ندارد!");

        var userDto = _common.Mapper.Map<UserDetailsDto>(user);

        return GeneralResult<UserDetailsDto>.Success(userDto);
    }
    public async Task<GeneralResult<UserDetailsDto>> GetUserByMobileNumberAsync(string mobileNumber, CancellationToken ct)
    {
        var user = await _uow.User.GetByFilterAsync(u => u.MobileNumber == mobileNumber, false, ct);
        if (user == null)
            throw new NotFoundException("کاربری با این شماره موبایل وجود ندارد!");

        var userDto = _common.Mapper.Map<UserDetailsDto>(user);

        return GeneralResult<UserDetailsDto>.Success(userDto);
    }

    // Command methods
    public async Task<GeneralResult<long>> CreateUserAsync(CreateUserAppDto command, CancellationToken ct)
    {
        // Check mobile number exist
        await _userDomainService.EnsureCanCreateUserAsync(command.MobileNumber, ct);

        var userPassHash = _common.Password.Hash(command.Password);
        var user = _common.Mapper.Map<User>(command, opt => opt.Items[nameof(User.PasswordHash)] = userPassHash);

        await _uow.User.AddAsync(user, ct);

        return GeneralResult<long>.Success(user.Id);
    }
    public async Task<GeneralResult> UpdateUserAsync(UpdateUserAppDto command, CancellationToken ct)
    {
        var user = await _uow.User.GetByIdAsync(command.UserId, true, ct);
        if (user.IsNullParameter())
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(UpdateUserAsync)} method!");

        user!.UpdateUser(command.Email, command.FirstName, command.LastName);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteUserAsync(DeleteUserAppDto command, CancellationToken ct)
    {
        // This method use SP (Stored Procedure)

        var user = await _uow.User.GetByIdAsync(command.UserId, false, ct);
        if (user.IsNullParameter())
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(SoftDeleteUserAsync)} method!");

        _common.Password.VerifyAndCheck(user!.PasswordHash, command.Password, "رمز عبور اشتباه است!");

        // Check user has org
        // Check user in org
        await _userDomainService.EnsureCanDeleteUserAsync(command.UserId, ct);

        // Delete User (SP)
        // Delete all UserTokens By UserId (SP)
        // Delete All Orgs By UserId (SP)
        // Delete All OrgMemberships By OrgId (SP)
        // Delete All OrgInvitation By OrgId (SP)
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
        var user = await _uow.User.GetByIdAsync(command.UserId, true, ct);
        if (user.IsNullParameter())
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(ChangePasswordUserAsync)} method!");

        _common.Password.VerifyAndCheck(user!.PasswordHash, command.OldPassword, "رمز عبور اشتباه است!");

        user.ChangeUserPassword(_common.Password.Hash(command.NewPassword));

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> IncreaseUserPointsAsync(long id, CancellationToken ct)
    {
        var user = await _uow.User.GetByIdAsync(id, true, ct);
        if (user.IsNullParameter())
            throw new Exception($"user by {id} ID was not found. in {nameof(IncreaseUserPointsAsync)} method!");

        user!.IncreaseOrDecreasePoints(_common.AppSettings.UserSetting.PositiveUserPoints);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> DecreaseUserPointsAsync(long id, CancellationToken ct)
    {
        var user = await _uow.User.GetByIdAsync(id, true, ct);
        if (user.IsNullParameter())
            throw new Exception($"user by {id} ID was not found. in {nameof(DecreaseUserPointsAsync)} method!");

        user!.IncreaseOrDecreasePoints(_common.AppSettings.UserSetting.NegativeUserPoints);

        return GeneralResult.Success();
    }
}