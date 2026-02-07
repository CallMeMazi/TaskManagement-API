using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Common.Settings;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Enums.Statuses;
using TaskManagement.Domin.Interface.Services;

namespace TaskManagement.Application.Services.Application;
public class AuthService : IAuthServiec
{
    private readonly ICommonService _commonService;
    private readonly IUserTokenDomainService _tokenDomainService;
    private readonly AppSettings _appSettings;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;


    public AuthService(ICommonService commonService, IUserTokenDomainService tokenDomainService, IUnitOfWork unitOfWork
        , AppSettings appSettings, IMapper mapper)
    {
        _commonService = commonService;
        _tokenDomainService = tokenDomainService;
        _appSettings = appSettings;
        _uow = unitOfWork;
        _mapper = mapper;
    }


    // Query methods
    public async Task<GeneralResult<List<UserTokenDetailsDto>>> GetUserActiveTokensAsync(int userId, CancellationToken ct)
    {
        var tokens = await _uow.UserToken.GetAllByFilterAsync(ut =>
            ut.UserId == userId
            && ut.TokenStatus == TokenStatus.Active,
            false,
            ct
        );

        if (tokens.IsNullParameter() || !tokens.Any())
            throw new Exception($"any tokens for {userId} UserId was not found. in {nameof(GetUserActiveTokensAsync)} method!");

        var tokensDto = _mapper.Map<List<UserTokenDetailsDto>>(tokens);

        return GeneralResult<List<UserTokenDetailsDto>>.Success(tokensDto);
    }
    public async Task<GeneralResult> ValidateAccessTokenAsync(validateUserTokenAppDto query, CancellationToken ct)
    {
        // Validate JWT (Expire date, Signature, algorithm)
        // Check current DeviceId(DB) with DeviceId in token
        // Check user security stamp(DB) with security stamp in token

        var SecurityStampResult = _commonService.Jwt.GetSecurityStampFromAccessToken(query.AccessToken, query.DeviceId);
        if (!SecurityStampResult.IsSuccess)
            throw new UnAuthorizedException(SecurityStampResult.Message);

        var aceessTokenHash = _commonService.Password.Hash(query.AccessToken);

        var token = await _uow.UserToken.GetByFilterAsync(ut =>
            ut.AccessTokenHash == aceessTokenHash,
            false,
            ct
        );

        if (token.IsNullParameter())
            throw new UnAuthorizedException("توکن نامعتبر است، لطفا مجددا لاگین کنید!");

        if (token!.TokenStatus != TokenStatus.Active || token.SecurityStamp != SecurityStampResult.Result)
            throw new UnAuthorizedException("توکن نامعتبر است، لطفا مجددا لاگین کنید!");

        return GeneralResult.Success();
    }

    // Command methods
    public async Task<GeneralResult<UserTokenDto>> RegisterUserAsync(RegisterUserTokenAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        var tokenResult = _commonService.Jwt.GenerateAccessTokenAndRefreshToken(command.user, command.DeviceId);
        if (!tokenResult.IsSuccess)
            throw new BadRequestException(tokenResult.Message);

        (string accessTokenHashed, string refreshTokenHashed) = HashAcceesTokenAndRefreshToken(tokenResult.Result!.AccessTokenHash, tokenResult.Result.RefreshTokenHash);

        var userToken = new UserToken(
            command.user.Id,
            accessTokenHashed,
            refreshTokenHashed,
            command.user.SecurityStamp,
            DateTime.Now.AddDays(_appSettings.JwtSetting.ExpirationDaysRefreshToken),
            command.DeviceId,
            command.UserIp,
            command.UserAgent
        );

        await _uow.UserToken.AddAsync(userToken, ct);

        var result = new UserTokenDto()
        {
            AccessTokenHash = tokenResult.Result.AccessTokenHash,
            RefreshTokenHash = tokenResult.Result.RefreshTokenHash,
        };

        return GeneralResult<UserTokenDto>.Success(result);
    }
    public async Task<GeneralResult<UserTokenDto>> LoginUserAsync(LoginUserAppDto command, CancellationToken ct)
    {
        var user = await _uow.User.GetByFilterAsync(u => u.MobileNumber == command.MobileNumber, false, ct);
        if (user.IsNullParameter())
            throw new NotFoundException("کاربری با این شماره موبایل پیدا نشد!");

        if (!_commonService.Password.Verify(user!.PasswordHash, command.Password))
            throw new NotFoundException("شماره موبایل یا رمز عبور اشتباه است!");

        await _tokenDomainService.EnsureCanLoginAsync(user.Id, ct);

        var tokenResult = _commonService.Jwt.GenerateAccessTokenAndRefreshToken(user!, command.DeviceId);
        if (!tokenResult.IsSuccess)
            throw new BadRequestException(tokenResult.Message);

        (string accessTokenHashed, string refreshTokenHashed) = HashAcceesTokenAndRefreshToken(tokenResult.Result!.AccessTokenHash, tokenResult.Result.RefreshTokenHash);

        var userToken = new UserToken(
            user!.Id,
            accessTokenHashed,
            refreshTokenHashed,
            user.SecurityStamp,
            DateTime.Now.AddDays(_appSettings.JwtSetting.ExpirationDaysRefreshToken),
            command.DeviceId,
            command.UserIp,
            command.UserAgent
        );

        await _uow.UserToken.AddAsync(userToken, ct);
        await _uow.SaveAsync(ct);

        var result = new UserTokenDto()
        {
            AccessTokenHash = tokenResult.Result.AccessTokenHash,
            RefreshTokenHash = tokenResult.Result.RefreshTokenHash,
        };

        return GeneralResult<UserTokenDto>.Success(result);
    }
    public async Task<GeneralResult> LogoutUserAsync(LogoutUserAppDto command, CancellationToken ct)
    {
        var token = await _uow.UserToken.GetUserTokenByFilterWithUserAsync(ut =>
            ut.TokenStatus == TokenStatus.Active
            && ut.DeviceId == command.DeviceId
            && ut.UserId == command.UserId,
            true,
            ct
        );
        if (token.IsNullParameter())
            throw new NotFoundException("توکنی برای شما با این اطلاعات یافت نشد!");

        var validateResult = _commonService.Jwt.ValidateAccessTokenAndGetPrincipal(command.AccessToken, command.DeviceId);
        if (!validateResult.IsSuccess)
            throw new BadRequestException(validateResult.Message);

        var commandAccessTokenHash = _commonService.Password.Hash(command.AccessToken);
        if (token!.AccessTokenHash != commandAccessTokenHash)
            throw new BadRequestException("توکن ارسالی شما نامعتبر است");

        token.RevokeToken();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult<UserTokenDto>> RefreshTokenAsync(RefreshUserTokenAppDto command, CancellationToken ct)
    {
        var token = await _uow.UserToken.GetUserTokenByFilterWithUserAsync(ut =>
            ut.TokenStatus == TokenStatus.Active
            && ut.DeviceId == command.DeviceId,
            true,
            ct
        );
        if (token.IsNullParameter())
            throw new NotFoundException("برای شما در این دستگاه یا مرورگر توکنی یافت نشد!");

        var commandRefreshTokenHash = _commonService.Password.Hash(command.RefreshToken);
        if (token!.RefreshTokenHash != commandRefreshTokenHash)
            throw new BadRequestException("رفرش توکن نامعتبر است!");

        await CheckSecurityStampAsync(token.User.SecurityStamp, token, ct);

        var newTokensResult = _commonService.Jwt.GenerateAccessTokenAndRefreshToken(token.User, command.DeviceId);
        if (!newTokensResult.IsSuccess)
            throw new BadRequestException(newTokensResult.Message);

        (string accessToken, string refreshToken) = HashAcceesTokenAndRefreshToken(newTokensResult.Result!.AccessTokenHash, newTokensResult.Result.RefreshTokenHash);

        token.RefreshToken(accessToken, refreshToken, _appSettings.JwtSetting.ExpirationDaysRefreshToken);
        await _uow.SaveAsync(ct);

        var result = new UserTokenDto()
        {
            AccessTokenHash = newTokensResult.Result.AccessTokenHash,
            RefreshTokenHash = newTokensResult.Result.RefreshTokenHash,
        };

        return GeneralResult<UserTokenDto>.Success(result);
    }
    public async Task<GeneralResult> RevokeTokenByDeviceIdAsync(RevokeUserTokenAppDto command, CancellationToken ct)
    {
        var token = await _uow.UserToken.GetByFilterAsync(ut =>
            ut.UserId == command.UserId
            && ut.TokenStatus == TokenStatus.Active
            && ut.DeviceId == command.Deviceid,
            true,
            ct
        );
        if (token.IsNullParameter())
            throw new NotFoundException("توکنی با این اطلاعات یافت نشد!");

        token!.RevokeToken();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RevokeAllTokensByUserIdAsync(int userId, bool isSaved, CancellationToken ct)
    {
        var tokens = await _uow.UserToken.GetAllByFilterAsync(ut =>
            ut.UserId == userId
            && ut.TokenStatus == TokenStatus.Active,
            true,
            ct
        );
        if (tokens.IsNullParameter() || !tokens.Any())
            return GeneralResult.Success();

        tokens.ForEach(ut =>
            ut.RevokeToken()
        );

        if (isSaved)
            await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RevokeAllTokensExceptCurrentByUserIdAsync(RevokeUserTokenAppDto command, bool isSaved, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        var tokens = await _uow.UserToken.GetAllByFilterAsync(ut =>
            ut.UserId == command.UserId
            && ut.DeviceId != command.Deviceid
            && ut.TokenStatus == TokenStatus.Active,
            true,
            ct
        );
        if (tokens.IsNullParameter() || !tokens.Any())
            return GeneralResult.Success();

        tokens.ForEach(ut =>
            ut.RevokeToken()
        );

        if (isSaved)
            await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }

    private (string accessToken, string refreshToken) HashAcceesTokenAndRefreshToken(string accessToken, string refreshToken)
    {
        var accessTokenHashed = _commonService.Password.Hash(accessToken);
        var refreshTokenHashed = _commonService.Password.Hash(refreshToken);

        return (accessTokenHashed, refreshTokenHashed);
    }
    private async System.Threading.Tasks.Task CheckSecurityStampAsync(string securityStamp, UserToken token, CancellationToken ct)
    {
        if (securityStamp != token.SecurityStamp)
        {
            token.RevokeToken();
            await _uow.SaveAsync(ct);
            throw new BadRequestException("توکن شما معتبر نیست، لطفا دوباره لاگین کنید!");
        }
    }
}
