using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Enums;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Common.Settings;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Services.Helper;
public class Jwtservice : IJwtService
{
    private readonly AppSettings _appSettings;


    public Jwtservice(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }


    public GeneralResult<string> GenerateAccessToken(User user, string deviceId)
    {
        var secretKey = Convert.FromBase64String(_appSettings.JwtSetting.SecretKey);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var encryptionkey = Encoding.UTF8.GetBytes(_appSettings.JwtSetting.Encryptkey);
        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var std = new SecurityTokenDescriptor
        {
            Issuer = _appSettings.JwtSetting.Issuer,
            Audience = _appSettings.JwtSetting.Audience,
            IssuedAt = DateTime.Now,
            Expires = DateTime.Now.AddMinutes(_appSettings.JwtSetting.ExpirationMinutesAccessToken),
            NotBefore = DateTime.Now.AddMinutes(_appSettings.JwtSetting.NotBeforeMinutes),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(GetClaims(user, deviceId))
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(std);
        var tokenString = handler.WriteToken(token);

        return GeneralResult<string>.Success(tokenString);
    }
    public GeneralResult<string> GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshtoken = Convert.ToBase64String(randomNumber);
        return GeneralResult<string>.Success(refreshtoken)!;
    }
    public GeneralResult<UserTokenDto> GenerateAccessTokenAndRefreshToken(User user, string deviceId)
    {
        var accessTokenResult = GenerateAccessToken(user, deviceId);
        if (!accessTokenResult.IsSuccess)
            return GeneralResult<UserTokenDto>.Failure(null, accessTokenResult.Message);

        var refreshTokenResult = GenerateRefreshToken();
        if (refreshTokenResult.IsSuccess)
            return GeneralResult<UserTokenDto>.Failure(null, refreshTokenResult.Message);

        return GeneralResult<UserTokenDto>.Success(new UserTokenDto()
        {
            AccessTokenHash = accessTokenResult.Result!,
            RefreshTokenHash = refreshTokenResult.Result!
        });
    }
    public GeneralResult<ClaimsPrincipal> ValidateAccessTokenAndGetPrincipal(string token, string deviceId)
    {
        try
        {
            var validationParameters = GetTokenValidationParameters();
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out var securityToken);
            if (!(securityToken is JwtSecurityToken jwt && jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase)))
                return GeneralResult<ClaimsPrincipal>.Failure(null, "توکن نامعتبر است!");

            if (!ValidationDeviceId(principal, deviceId))
                return GeneralResult<ClaimsPrincipal>.Failure(null, "توکن متعلق به این دستگاه نیست!");

            return GeneralResult<ClaimsPrincipal>.Success(principal);
        }
        catch (SecurityTokenExpiredException)
        {
            return GeneralResult<ClaimsPrincipal>.Failure(null, "توکن منقضی شده است!");
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return GeneralResult<ClaimsPrincipal>.Failure(null, "امضای توکن نامعتبر است!");
        }
        catch (SecurityTokenException)
        {
            return GeneralResult<ClaimsPrincipal>.Failure(null, "توکن معتبر نیست!");
        }
        catch (Exception ex)
        {
            throw new AppException(
                HttpStatusCode.InternalServerError,
                ResultStatus.ServerError,
                "خطایی هنگام اعتبارسنجی توکن اتفاق افتاد!",
                null,
                ex
            );
        }

        bool ValidationDeviceId(ClaimsPrincipal principal, string currentDeviceId)
        {
            var deviceId = principal.FindFirst("deviceId")!.Value;
            return deviceId.Equals(currentDeviceId);
        }
    }
    public GeneralResult<int> GetUserIdFromAccessToken(string token, string deviceId)
    {
        var principalResult = ValidateAccessTokenAndGetPrincipal(token, deviceId);
        if (!principalResult.IsSuccess)
            return GeneralResult<int>.Failure(default, principalResult.Message);

        var userId = principalResult.Result!.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userId.IsNullParameter())
            throw new Exception($"The UserID not found in token, error in {nameof(GetUserIdFromAccessToken)} method!");

        if (int.TryParse(userId, out var id))
            return GeneralResult<int>.Success(id);

        return GeneralResult<int>.Failure(default, "مقدار شناسه درون توکن نامعتبر است!");
    }
    public GeneralResult<string> GetSecurityStampFromAccessToken(string token, string deviceId)
    {
        var principalResult = ValidateAccessTokenAndGetPrincipal(token, deviceId);
        if (!principalResult.IsSuccess)
            return GeneralResult<string>.Failure(null, principalResult.Message);

        var securityStamp = principalResult.Result!.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType)?.Value;
        if (securityStamp.IsNullParameter())
            throw new Exception($"The UserID not found in token, error in {nameof(GetUserIdFromAccessToken)} method!");

        return GeneralResult<string>.Success(securityStamp!);
    }
    public GeneralResult<string> GetClaimValueByAccessToken(string token, string claimType, string deviceId)
    {
        var principalResul = ValidateAccessTokenAndGetPrincipal(token, deviceId);
        if (!principalResul.IsSuccess)
            return GeneralResult<string>.Failure(null, principalResul.Message);

        var claim = principalResul.Result!.FindFirst(claimType)?.Value;
        if (claim.IsNullParameter())
            return GeneralResult<string>.Failure(null, "مقدار مورد نظر در توکن یافت نشد!");

        return GeneralResult<string>.Success(claim!);
    }

    private IEnumerable<Claim> GetClaims(User user, string deviceId)
    {
        yield return new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString());
        yield return new Claim(ClaimTypes.MobilePhone, user.MobileNumber);
        yield return new Claim(new ClaimsIdentityOptions().SecurityStampClaimType, user.SecurityStamp);
        yield return new Claim("deviceId", deviceId);
    }
    private TokenValidationParameters GetTokenValidationParameters()
    {
        var secretKey = Convert.FromBase64String(_appSettings.JwtSetting.SecretKey);
        var encryptionkey = Encoding.UTF8.GetBytes(_appSettings.JwtSetting.Encryptkey);

        return new TokenValidationParameters
        {
            ValidateIssuer = !_appSettings.JwtSetting.Issuer.IsNullParameter(),
            ValidIssuer = _appSettings.JwtSetting.Issuer,

            ValidateAudience = !_appSettings.JwtSetting.Audience.IsNullParameter(),
            ValidAudience = _appSettings.JwtSetting.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),

            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
            RequireExpirationTime = true,

            TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
        };
    }
}
