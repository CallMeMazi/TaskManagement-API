using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Main;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommonService _commonService;
    private readonly IMapper _mapper;


    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, ICommonService commonService
        , IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _commonService = commonService;
        _mapper = mapper;
    }


    public async Task<GeneralResult<UserDetailsDto>> GetUserByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken)
    {
        if (mobileNumber.IsNullParameter())
            throw new BadRequestException("شماره موبایل خالی است!");

        var user = await _userRepository.GetUserDtoByFilterAsync(u => u.MobileNumber == mobileNumber, cancellationToken);

        if (user == null)
            throw new NotFoundException("کاربری با این شماره موبایل وجود ندارد!");

        return GeneralResult<UserDetailsDto>.Success(user);
    }
    public async Task<GeneralResult> CreateUserAsync(CreateUserAppDto command, CancellationToken cancellationToken)
    {
        if (await _userRepository.IsUserExistByFilterAsync(u => u.MobileNumber == command.MobileNumber, cancellationToken))
            throw new BadRequestException("کاربری با این شماره موبایل وجود دارد!");

        command.Password = _commonService.Password.Hash(command.Password);

        var user = _mapper.Map<User>(command);

        await _userRepository.AddUserAsync(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> UpdateUserAsync(UpdateUserAppDto command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(command.UserId, true, cancellationToken);
        if (user == null)
            throw new Exception("مشکلی پیش آمده!");

        user.UpdateUser(command.Email, command.FirstName, command.LastName);

        _userRepository.UpdateUser(user);
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteUserAsync(DeleteUserAppDto command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(command.UserId, true, cancellationToken);
        if (user == null)
            throw new Exception("مشکلی پیش آمده!");

        user.SoftDelete();

        _userRepository.UpdateUser(user);
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangePasswordUserAsync(ChangePasswordUserAppDto command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(command.UserId, true, cancellationToken);
        if (user == null)
            throw new Exception("مشکلی پیش آمده!");

        if (!_commonService.Password.Verify(user.PasswordHash, command.OldPassword))
            throw new BadRequestException("رمز عبور اشتباه است!");

        command.NewPassword = _commonService.Password.Hash(command.NewPassword);
        user.ChangeUserPassword(command.NewPassword);
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success("رمز عبور با موفقیت بروزرسانی شد.");
    }
}
