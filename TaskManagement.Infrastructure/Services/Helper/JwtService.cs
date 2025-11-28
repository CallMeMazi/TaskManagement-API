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


    public GeneralResult<string?> GenerateAccessToken(User user, string currentIp, string currentAgent)
    {
        var validationResult = Validation(user, currentIp, currentAgent);
        if (!validationResult.IsSuccess)
            return validationResult;

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
            Subject = new ClaimsIdentity(_GetClaims(user, currentIp, currentAgent))
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(std);
        var tokenString = handler.WriteToken(token);

        return GeneralResult<string?>.Success(tokenString);

        GeneralResult<string?> Validation(User user, string currentIp, string currentAgent)
        {
            if (user.IsNullParameter())
                return GeneralResult<string?>.Failure(null, "کاربر برای ساخت توکن معتبر نیست!");

            if (currentIp.IsNullParameter() || !IPAddress.TryParse(currentIp, out _))
                return GeneralResult<string?>.Failure(null, "آیپی شما نامعتبر است!");

            if (currentAgent.IsNullParameter())
                return GeneralResult<string?>.Failure(null, "اطلاعات دستگاه شما نامعتبر است!");

            return GeneralResult<string?>.Success(null);
        }
    }
    public GeneralResult<string?> GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshtoken = Convert.ToBase64String(randomNumber);
        return GeneralResult<string>.Success(refreshtoken);
    }
    public GeneralResult<UserTokenDto?> GenerateAccessTokenAndRefreshToken(User user, string currentIp, string currentAgent)
    {
        var accessTokenResult = GenerateAccessToken(user, currentIp, currentAgent);
        if (accessTokenResult.IsSuccess)
        {
            var refreshTokenResult = GenerateRefreshToken();
            if (refreshTokenResult.IsSuccess)
                return GeneralResult<UserTokenDto?>.Success(new UserTokenDto()
                {
                    AccessTokenHash = accessTokenResult.Result!,
                    RefreshTokenHash = refreshTokenResult.Result!
                });
            else
                return GeneralResult<UserTokenDto?>.Failure(null, refreshTokenResult.Message);
        }

        return GeneralResult<UserTokenDto?>.Failure(null, accessTokenResult.Message);
    }

    public GeneralResult<ClaimsPrincipal?> ValidateAccessTokenAndGetPrincipal(string token, string currentIp, string currentAgent, bool getPrincipal = true)
    {
        var validateResult = Validation(token, currentIp, currentAgent);
        if (!validateResult.IsSuccess)
            return validateResult;

        try
        {
            var validationParameters = _GetTokenValidationParameters();
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out var securityToken);
            if (!(securityToken is JwtSecurityToken jwt && jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase)))
                return GeneralResult<ClaimsPrincipal?>.Failure(null, "توکن نامعتبر است!");

            if (!ValidationIpAndAgent(principal, currentIp, currentAgent))
                return GeneralResult<ClaimsPrincipal?>.Failure(null, "توکن متعلق به این دستگاه نیست!");

            if (getPrincipal)
                return GeneralResult<ClaimsPrincipal?>.Success(principal);

            return GeneralResult<ClaimsPrincipal?>.Success(null);
        }
        catch (SecurityTokenExpiredException)
        {
            return GeneralResult<ClaimsPrincipal?>.Failure(null, "توکن منقضی شده است!");
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return GeneralResult<ClaimsPrincipal?>.Failure(null, "امضای توکن نامعتبر است!");
        }
        catch (SecurityTokenException)
        {
            return GeneralResult<ClaimsPrincipal?>.Failure(null, "توکن معتبر نیست!");
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

        GeneralResult<ClaimsPrincipal?> Validation(string token, string currentIp, string currentAgent)
        {
            if (token.IsNullParameter())
                return GeneralResult<ClaimsPrincipal?>.Failure(null, "مقدار توکن خالی است!");

            if (currentIp.IsNullParameter() || !IPAddress.TryParse(currentIp, out _))
                return GeneralResult<ClaimsPrincipal?>.Failure(null, "آیپی شما نامعتبر است!");

            if (currentAgent.IsNullParameter())
                return GeneralResult<ClaimsPrincipal?>.Failure(null, "اطلاعات دستگاه شما نامعتبر است!");

            return GeneralResult<ClaimsPrincipal?>.Success(null);
        }
        bool ValidationIpAndAgent(ClaimsPrincipal principal, string currentIp, string currentAgent)
        {
            var tokenIp = principal.FindFirst("ip")!.Value;
            var tokenAgent = principal.FindFirst("agent")!.Value;
            if (!(tokenIp.Equals(currentIp) && tokenAgent.Equals(currentAgent)))
                return false;

            return true;
        }
    }

    public GeneralResult<int?> GetUserIdFromAccessToken(string token, string currentIp, string currentAgent)
    {
        var principalResult = ValidateAccessTokenAndGetPrincipal(token, currentIp, currentAgent);
        if (!principalResult.IsSuccess)
            return GeneralResult<int?>.Failure(null, principalResult.Message);

        var sub = principalResult.Result!.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (int.TryParse(sub, out var id))
            return GeneralResult<int?>.Success(id);

        return GeneralResult<int?>.Failure(null, "مقدار شناسه درون توکن نامعتبر است!");
    }
    public GeneralResult<string?> GetSecurityStampFromAccessToken(string token, string currentIp, string currentAgent)
    {
        var principalResult = ValidateAccessTokenAndGetPrincipal(token, currentIp, currentAgent);
        if (!principalResult.IsSuccess)
            return GeneralResult<string?>.Failure(null, principalResult.Message);

        var securityStamp = principalResult.Result!.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType)?.Value;

        return GeneralResult<string?>.Success(securityStamp);
    }
    public GeneralResult<string?> GetClaimValueByAccessToken(string token, string claimType, string currentIp, string currentAgent)
    {
        var principalResul = ValidateAccessTokenAndGetPrincipal(token, currentIp, currentAgent);
        if (!principalResul.IsSuccess)
            return GeneralResult<string?>.Failure(null, principalResul.Message);

        var claim = principalResul.Result!.FindFirst(claimType)?.Value;
        if (claim == null)
            return GeneralResult<string?>.Failure(null, "مقدار مورد نظر در توکن یافت نشد!");

        return GeneralResult<string?>.Success(claim);
    }

    private IEnumerable<Claim> _GetClaims(User user, string ip, string agent)
    {
        Validation(user, ip, agent);

        yield return new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString());
        yield return new Claim(ClaimTypes.MobilePhone, user.MobileNumber);
        yield return new Claim(new ClaimsIdentityOptions().SecurityStampClaimType, user.SecurityStamp);
        yield return new Claim("ip", ip);
        yield return new Claim("agent", agent);

        void Validation(User user, string ip, string agent)
        {
            if (user.MobileNumber.IsNullParameter() || user.SecurityStamp.IsNullParameter())
                throw new AppException();

            if (ip.IsNullParameter() || !IPAddress.TryParse(ip, out _))
                throw new BadRequestException("آیپی شما نامعتبر است!");

            if (agent.IsNullParameter())
                throw new BadRequestException("اطلاعات دستگاه شما خالی است!");
        }
    }
    private TokenValidationParameters _GetTokenValidationParameters()
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
