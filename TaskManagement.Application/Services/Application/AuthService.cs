using TaskManagement.Application.DTOs.InternalDTOs.UserToken;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Application.Utilities.Exceptions;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Interface.Services;

namespace TaskManagement.Application.Services.Application;

public class AuthService : IAuthServiec
{
    private readonly ICommonService _common;
    private readonly IUserTokenDomainService _tokenDomainService;
    private readonly IUnitOfWork _uow;

    public AuthService(ICommonService common, IUserTokenDomainService tokenDomainService, IUnitOfWork unitOfWork)
    {
        _common = common;
        _tokenDomainService = tokenDomainService;
        _uow = unitOfWork;
    }

    // Query methods
    public async Task<List<UserTokenDetailsDto>> GetUserActiveTokensAsync(long userId, CancellationToken ct)
    {
        var tokens = await _uow.UserToken.GetAllByFilterAsync(ut =>
            ut.UserId == userId
            && ut.TokenStatus == TokenStatusType.Active,
            false,
            ct
        );
        if (tokens.IsNullParameter() || !tokens.Any())
            throw new Exception($"any tokens for {userId} UserId was not found. in {nameof(GetUserActiveTokensAsync)} method!");

        var tokensDto = _common.Mapper.Map<List<UserTokenDetailsDto>>(tokens);

        return tokensDto;
    }
    public async System.Threading.Tasks.Task ValidateAccessTokenAsync(ValidateUserTokenAppDto query, CancellationToken ct)
    {
        // Validate JWT (Expire date, Signature, algorithm)
        // Check current DeviceId(DB) with DeviceId in token
        // Check user security stamp(DB) with security stamp in token

        var SecurityStampResult = _common.Jwt.GetSecurityStampFromAccessToken(query.AccessToken, query.DeviceId);
        if (!SecurityStampResult.IsSuccess)
            throw new UnAuthorizedException(SecurityStampResult.Message);

        var aceessTokenHashResult = _common.Password.Hash(query.AccessToken);

        var token = await _uow.UserToken.GetByFilterAsync(ut =>
            ut.AccessTokenHash == aceessTokenHashResult.Result,
            false,
            ct
        );

        if (token.IsNullParameter())
            throw new UnAuthorizedException("توکن نامعتبر است، لطفا مجددا لاگین کنید!");

        if (token!.TokenStatus != TokenStatusType.Active || token.SecurityStamp != SecurityStampResult.Result)
            throw new UnAuthorizedException("توکن نامعتبر است، لطفا مجددا لاگین کنید!");
    }

    // Command methods
    public async Task<UserTokenDto> RegisterUserAsync(RegisterUserTokenAppDto command, CancellationToken ct)
    {
        var user = await _uow.User.GetByIdAsync(command.UserId, false, ct);

        var tokenResult = _common.Jwt.GenerateAccessTokenAndRefreshToken
            (new GenerateTokensInternalDto(user!.Id, user.MobileNumber, user.SecurityStamp, command.DeviceId));

        if (!tokenResult.IsSuccess)
            throw new ValidationFailureException(tokenResult.Message);

        (string accessTokenHashed, string refreshTokenHashed) = HashAcceesTokenAndRefreshToken(tokenResult.Result!.AccessTokenHash, tokenResult.Result.RefreshTokenHash);

        var userToken = new UserToken(
            command.UserId,
            accessTokenHashed,
            refreshTokenHashed,
            user.SecurityStamp,
            DateTime.Now.AddDays(_common.AppSettings.JwtSetting.ExpirationDaysRefreshToken),
            command.DeviceId,
            command.UserIp,
            command.UserAgent
        );

        await _uow.UserToken.AddAsync(userToken, ct);

        var result = new UserTokenDto(tokenResult.Result.AccessTokenHash, tokenResult.Result.RefreshTokenHash);

        return result;
    }
    public async Task<UserTokenDto> LoginUserAsync(LoginUserAppDto command, CancellationToken ct)
    {
        var user = await _uow.User.GetByFilterAsync(u => u.MobileNumber == command.MobileNumber, false, ct);
        if (user.IsNullParameter())
            throw new NotFoundException("کاربری با این شماره موبایل پیدا نشد!");

        _common.Password.VerifyAndCheck(user!.PasswordHash, command.Password, "شماره موبایل یا رمز عبور اشتباه است!");

        // Check user active device count
        await _tokenDomainService.EnsureCanLoginAsync(user.Id, ct);

        var tokenResult = _common.Jwt.GenerateAccessTokenAndRefreshToken
            (new GenerateTokensInternalDto(user.Id, command.MobileNumber, user.SecurityStamp, command.DeviceId));

        if (!tokenResult.IsSuccess)
            throw new ValidationFailureException(tokenResult.Message);

        (string accessTokenHashed, string refreshTokenHashed) = HashAcceesTokenAndRefreshToken(tokenResult.Result!.AccessTokenHash, tokenResult.Result.RefreshTokenHash);

        var userToken = new UserToken(
            user!.Id,
            accessTokenHashed,
            refreshTokenHashed,
            user.SecurityStamp,
            DateTime.Now.AddDays(_common.AppSettings.JwtSetting.ExpirationDaysRefreshToken),
            command.DeviceId,
            command.UserIp,
            command.UserAgent
        );

        await _uow.UserToken.AddAsync(userToken, ct);

        var result = new UserTokenDto(tokenResult.Result.AccessTokenHash, tokenResult.Result.RefreshTokenHash);

        return result;
    }
    public async System.Threading.Tasks.Task LogoutUserAsync(LogoutUserAppDto command, CancellationToken ct)
    {
        var token = await _uow.UserToken.GetUserTokenByFilterWithUserAsync(ut =>
            ut.TokenStatus == TokenStatusType.Active
            && ut.DeviceId == command.DeviceId
            && ut.UserId == command.UserId,
            true,
            ct
        );
        if (token.IsNullParameter())
            throw new NotFoundException("توکنی برای شما با این اطلاعات یافت نشد!");

        var validateResult = _common.Jwt.ValidateAccessTokenAndGetPrincipal(command.AccessToken, command.DeviceId);
        if (!validateResult.IsSuccess)
            throw new ValidationFailureException(validateResult.Message);

        var commandAccessTokenHashResult = _common.Password.Hash(command.AccessToken);
        if (token!.AccessTokenHash != commandAccessTokenHashResult.Result)
            throw new ValidationFailureException("توکن ارسالی شما نامعتبر است");

        token.RevokeToken();
    }
    public async Task<UserTokenDto> RefreshTokenAsync(RefreshUserTokenAppDto command, CancellationToken ct)
    {
        var token = await _uow.UserToken.GetUserTokenByFilterWithUserAsync(ut =>
            ut.TokenStatus == TokenStatusType.Active
            && ut.DeviceId == command.DeviceId,
            true,
            ct
        );
        if (token.IsNullParameter())
            throw new NotFoundException("برای شما در این دستگاه یا مرورگر توکنی یافت نشد!");

        var commandRefreshTokenHashResult = _common.Password.Hash(command.RefreshToken);
        if (token!.RefreshTokenHash != commandRefreshTokenHashResult.Result)
            throw new ValidationFailureException("رفرش توکن نامعتبر است!");

        if (token.User.SecurityStamp != token.SecurityStamp)
        {
            token.RevokeToken();
            await _uow.SaveAsync(ct);
            throw new ConflictException("اطلاعات کاربری بروزرسانی شده، لطفا دوباره وارد شوید!");
        }

        var newTokensResult = _common.Jwt.GenerateAccessTokenAndRefreshToken
            (new GenerateTokensInternalDto(token.User.Id, token.User.MobileNumber, token.User.SecurityStamp, command.DeviceId));

        if (!newTokensResult.IsSuccess)
            throw new ValidationFailureException(newTokensResult.Message);

        (string accessToken, string refreshToken) = HashAcceesTokenAndRefreshToken(newTokensResult.Result!.AccessTokenHash, newTokensResult.Result.RefreshTokenHash);

        token.RefreshToken(accessToken, refreshToken, _common.AppSettings.JwtSetting.ExpirationDaysRefreshToken);

        var result = new UserTokenDto(newTokensResult.Result.AccessTokenHash, newTokensResult.Result.RefreshTokenHash);

        return result;
    }
    public async System.Threading.Tasks.Task RevokeTokenByDeviceIdAsync(RevokeUserTokenAppDto command, CancellationToken ct)
    {
        var token = await _uow.UserToken.GetByFilterAsync(ut =>
            ut.UserId == command.UserId
            && ut.TokenStatus == TokenStatusType.Active
            && ut.DeviceId == command.DeviceId,
            true,
            ct
        );
        if (token.IsNullParameter())
            throw new NotFoundException("توکنی با این اطلاعات یافت نشد!");

        token!.RevokeToken();
    }
    public async System.Threading.Tasks.Task RevokeAllTokensByUserIdAsync(long userId, CancellationToken ct)
    {
        var tokens = await _uow.UserToken.GetAllByFilterAsync(ut =>
            ut.UserId == userId
            && ut.TokenStatus == TokenStatusType.Active,
            true,
            ct
        );
        if (tokens.IsNullParameter() || !tokens.Any())
            return;

        tokens.ForEach(ut =>
            ut.RevokeToken()
        );
    }
    public async System.Threading.Tasks.Task RevokeAllTokensExceptCurrentByUserIdAsync(RevokeUserTokenAppDto command, CancellationToken ct)
    {
        var tokens = await _uow.UserToken.GetAllByFilterAsync(ut =>
            ut.UserId == command.UserId
            && ut.DeviceId != command.DeviceId
            && ut.TokenStatus == TokenStatusType.Active,
            true,
            ct
        );
        if (tokens.IsNullParameter() || !tokens.Any())
            return;

        tokens.ForEach(ut =>
            ut.RevokeToken()
        );
    }

    private (string accessToken, string refreshToken) HashAcceesTokenAndRefreshToken(string accessToken, string refreshToken)
    {
        var accessTokenHashedResult = _common.Password.Hash(accessToken);
        var refreshTokenHashedResult = _common.Password.Hash(refreshToken);

        return (accessTokenHashedResult.Result!, refreshTokenHashedResult.Result!);
    }
}
