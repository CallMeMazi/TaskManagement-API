using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Common.Settings;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Application.Services;
public class AuthService
{
    private readonly ICommonService _commonService;
    private readonly AppSettings _appSettings;
    private readonly IUnitOfWork _unitOfWork;


    public AuthService(ICommonService commonService, IUnitOfWork unitOfWork, AppSettings appSettings)
    {
        _commonService = commonService;
        _appSettings = appSettings;
        _unitOfWork = unitOfWork;
    }


    public async Task<GeneralResult<List<UserTokenDetails>>> GetUserActiveTokensAsync(int userId, CancellationToken cancellationToken)
    {
        if (userId <= 0)
            throw new Exception($"the ID of user is invalide. Exception in {nameof(GetUserActiveTokensAsync)} method!");

        var tokens = await _unitOfWork.UserTokenRepository.
            GetAllUserTokenDtosByFilterAsync(ut =>
                ut.UserId == userId
                && ut.TokenStatus == TokenStatus.Active,
                cancellationToken
            );

        if (tokens.IsNullParameter() || !tokens.Any())
            throw new Exception($"any tokens for {userId} UserId was not found. in {nameof(GetUserActiveTokensAsync)} method!");

        return GeneralResult<List<UserTokenDetails>>.Success(tokens)!;
    }
    public async Task<GeneralResult<UserTokenDto>> RegisterUserAsync(RegisterUserTokenAppDto command, CancellationToken cancellationToken)
    {
        var tokenResult = _commonService.Jwt.GenerateAccessTokenAndRefreshToken(command.user, command.UserIp, command.UserAgent);
        if (!tokenResult.IsSuccess)
            throw new BadRequestException(tokenResult.Message);

        (string accessTokenHashed, string refreshTokenHashed) = HashAcceesTokenAndRefreshToken(tokenResult.Result!.AccessToken, tokenResult.Result.RefreshToken);

        var userToken = new UserToken(
            command.user.Id,
            accessTokenHashed,
            refreshTokenHashed,
            command.user.SecurityStamp,
            DateTime.Now.AddDays(_appSettings.JwtSetting.ExpirationDaysRefreshToken),
            command.UserIp,
            command.UserAgent
        );

        await _unitOfWork.UserTokenRepository.AddUserTokenAsync(userToken, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var result = new UserTokenDto()
        {
            AccessToken = tokenResult.Result.AccessToken,
            RefreshToken = tokenResult.Result.RefreshToken,
        };

        return GeneralResult<UserTokenDto>.Success(result)!;
    }
    public async Task<GeneralResult<UserTokenDto>> LoginUserAsync(LoginUserAppDto command, CancellationToken cancellationToken)
    {
        if (command.IsNullParameter())
            throw new BadRequestException("فرم لاگین خالی است!");

        var user = await VerifyAndGetUserAsync(command, cancellationToken);

        var tokenResult = _commonService.Jwt.GenerateAccessTokenAndRefreshToken(user, command.UserIp, command.UserAgent);
        if (!tokenResult.IsSuccess)
            throw new BadRequestException(tokenResult.Message);

        (string accessTokenHashed, string refreshTokenHashed) = HashAcceesTokenAndRefreshToken(tokenResult.Result!.AccessToken, tokenResult.Result.RefreshToken);

        var userToken = new UserToken(
            user.Id,
            accessTokenHashed,
            refreshTokenHashed,
            user.SecurityStamp,
            DateTime.Now.AddDays(_appSettings.JwtSetting.ExpirationDaysRefreshToken),
            command.UserIp,
            command.UserAgent
        );

        await _unitOfWork.UserTokenRepository.AddUserTokenAsync(userToken, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var result = new UserTokenDto()
        {
            AccessToken = tokenResult.Result.AccessToken,
            RefreshToken = tokenResult.Result.RefreshToken,
        };

        return GeneralResult<UserTokenDto>.Success(result)!;
    }
    public async Task<GeneralResult> LogoutUserAsync(LogoutUserAppDto command, CancellationToken cancellationToken)
    {
        if (command.IsNullParameter())
            throw new BadRequestException("فرم لاگ اوت خالی است!");

        var token = await _unitOfWork.UserTokenRepository.
            GetUserTokenByFilterAsync(ut => 
                ut.TokenStatus == TokenStatus.Active 
                && ut.UserIp == command.UserIp 
                && ut.UserAgent == command.UserAgent 
                && ut.UserId == command.UserId,
                true,
                cancellationToken
            );

        if (token.IsNullParameter())
            throw new NotFoundException("توکنی برای شما با این آیپی و اطلاعات یافت نشد!");

        var validateResult = _commonService.Jwt.ValidateAccessTokenAndGetPrincipal(command.AccessToken, command.UserIp, command.UserAgent, false);
        if (!validateResult.IsSuccess)
            throw new BadRequestException(validateResult.Message);

        var commandAccessTokenHash = _commonService.Password.Hash(command.AccessToken);
        if (token!.AccessTokenHash != commandAccessTokenHash)
            throw new BadRequestException("توکن ارسالی شما نامعتبر است");

        token.RevokeToken();

        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult<UserTokenDto>.Success();
    }
    public async Task<GeneralResult<UserTokenDto>> RefreshTokenAsync(RefreshUserTokenAppDto command, CancellationToken cancellationToken)
    {
        if (command.IsNullParameter())
            throw new BadRequestException("فرم رفرش توکن خالی است!");

        var token = await _unitOfWork.UserTokenRepository.
           GetUserTokenByFilterAsync(ut =>
               ut.TokenStatus == TokenStatus.Active
               && ut.UserIp == command.UserIp
               && ut.UserAgent == command.UserAgent,
               true,
               cancellationToken
           );

        if (token.IsNullParameter())
            throw new NotFoundException("برای شما در این دستگاه یا مرورگر توکنی یافت نشد!");

        var commandRefreshTokenHash = _commonService.Password.Hash(command.RefreshToken);
        if (token!.RefreshTokenHash != commandRefreshTokenHash)
            throw new BadRequestException("رفرش توکن نامعتبر است!");

        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(token.UserId, cancellationToken: cancellationToken);
        if (user.IsNullParameter())
            throw new KeyNotFoundException($"Any User with {token.UserId} ID not found! exception in {nameof(RefreshTokenAsync)} method.");

        await CheckSecurityStampAsync(user!, token, cancellationToken);

        var newTokensResult = _commonService.Jwt.GenerateAccessTokenAndRefreshToken(user!, command.UserIp, command.UserAgent);
        if (!newTokensResult.IsSuccess)
            throw new BadRequestException(newTokensResult.Message);

        (string accessToken, string refreshToken) = HashAcceesTokenAndRefreshToken(newTokensResult.Result!.AccessToken, newTokensResult.Result.RefreshToken);

        token.RefreshToken(accessToken, refreshToken, _appSettings.JwtSetting.ExpirationDaysRefreshToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var result = new UserTokenDto()
        {
            AccessToken = newTokensResult.Result.AccessToken,
            RefreshToken = newTokensResult.Result.RefreshToken,
        };

        return GeneralResult<UserTokenDto>.Success(result)!;
    }
    public async Task<GeneralResult> RevokeTokenByUserIdAndIpAsync(RevokeUserTokenAppDto command, CancellationToken cancellationToken)
    {
        if (command.IsNullParameter())
            throw new BadRequestException("فرم خروج دیوایس خالی است!");

        var token = await _unitOfWork.UserTokenRepository.
            GetUserTokenByFilterAsync(ut =>
                ut.UserId == command.UserId
                && ut.TokenStatus == TokenStatus.Active
                && ut.UserIp == command.UserIp
                && ut.UserAgent == command.UserAgent,
                true,
                cancellationToken
            );

        if (token.IsNullParameter())
            throw new NotFoundException("توکنی با این اطلاعات یافت نشد!");

        token!.RevokeToken();
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RevokeAllTokensByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        if (userId <= 0)
            throw new Exception($"the ID of user is invalide. Exception in {nameof(RevokeAllTokensByUserIdAsync)} method!");

        var tokens = await _unitOfWork.UserTokenRepository.
            GetAllUserTokensByFilterAsync(ut =>
                ut.UserId == userId
                && ut.TokenStatus == TokenStatus.Active,
                true,
                cancellationToken
            );

        if (tokens.IsNullParameter() || !tokens.Any())
            return GeneralResult.Success();

        tokens.ForEach(ut =>
            ut.RevokeToken()
        );
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RevokeAllTokensExceptCurrentAsync(RevokeUserTokenAppDto command, CancellationToken cancellationToken)
    {
        if (command.IsNullParameter())
            throw new Exception($"Date is invalide. Exception in {nameof(RevokeAllTokensExceptCurrentAsync)} method!");

        var tokens = await _unitOfWork.UserTokenRepository.
            GetAllUserTokensByFilterAsync(ut =>
                ut.UserId == command.UserId
                && (ut.UserIp != command.UserIp || ut.UserAgent != command.UserAgent)
                && ut.TokenStatus == TokenStatus.Active,
                true,
                cancellationToken
            );

        if (tokens.IsNullParameter() || !tokens.Any())
            return GeneralResult.Success();

        tokens.ForEach(ut =>
            ut.RevokeToken()
        );
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ValidateAccessTokenAsync(validateUserTokenAppDto command, CancellationToken cancellationToken)
    {
        if (command.IsNullParameter())
            throw new BadRequestException("فرم اعتبارسنجی توکن خالی است!");

        var SecurityStampResult = _commonService.Jwt.GetSecurityStampFromAccessToken(command.AccessToken, command.UserIp, command.UserAgent);
        if (!SecurityStampResult.IsSuccess)
            throw new UnAuthorizedException(SecurityStampResult.Message);

        var aceessTokenHash = _commonService.Password.Hash(command.AccessToken);

        var token = await _unitOfWork.UserTokenRepository.
            GetUserTokenByFilterAsync(ut =>
                ut.AccessTokenHash == aceessTokenHash,
                true,
                cancellationToken
            );

        if (token.IsNullParameter())
            throw new Exception($"token not found. in {nameof(ValidateAccessTokenAsync)} method!");

        if (token!.TokenStatus != TokenStatus.Active || token.SecurityStamp != SecurityStampResult.Result)
            throw new UnAuthorizedException("توکن نامعتبر است، لطفا مجددا لاگین کنید!");

        return GeneralResult.Success();
    }

    private async Task<User> VerifyAndGetUserAsync(LoginUserAppDto command, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetUserByFilterAsync(u => u.MobileNumber == command.MobileNumber, cancellationToken: cancellationToken);
        if (user.IsNullParameter())
            throw new NotFoundException("شماره موبایل یا رمز عبور اشتباه است!");

        if (!_commonService.Password.Verify(user!.PasswordHash, command.Password))
            throw new NotFoundException("شماره موبایل یا رمز عبور اشتباه است!");

        var userDeviceCount = await GetUserActiveDeviceCountAsync(user.Id);
        if (userDeviceCount >= 3)
            throw new BadRequestException("نمیتوانید با بیشتر از سه دستگاه یا مرورگر متفاوت وارد شوید!");

        return user;
    }
    private Task<int> GetUserActiveDeviceCountAsync(int userId)
        => _unitOfWork.UserTokenRepository.GetCountByFilterAsync(ut => ut.UserId == userId && ut.TokenStatus == TokenStatus.Active);
    private (string accessToken, string refreshToken) HashAcceesTokenAndRefreshToken(string accessToken, string refreshToken)
    {
        var accessTokenHashed = _commonService.Password.Hash(accessToken);
        var refreshTokenHashed = _commonService.Password.Hash(refreshToken);

        return (accessTokenHashed, refreshTokenHashed);
    }
    private async System.Threading.Tasks.Task CheckSecurityStampAsync(User user, UserToken token, CancellationToken cancellationToken)
    {
        if (user.SecurityStamp != token.SecurityStamp)
        {
            token.RevokeToken();
            await _unitOfWork.SaveAsync(cancellationToken);
            throw new BadRequestException("توکن شما معتبر نیست، لطفا دوباره لاگین کنید!");
        }
    }
}
