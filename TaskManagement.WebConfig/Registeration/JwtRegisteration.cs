using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Application.Utilities.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Common.Settings;

namespace TaskManagement.WebConfig.Registeration;

public static class JwtRegisteration
{
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterJwtBearer(AppSettings appSettings)
        {
            services.RegisterJwtAuthentication(appSettings);

            return services;
        }
    }

    private static IServiceCollection RegisterJwtAuthentication(this IServiceCollection services, AppSettings appSettings)
    {
        // Add JWT authentication configuration
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var secretkey = Convert.FromBase64String(appSettings.JwtSetting.SecretKey);
            var encryptionKey = Encoding.UTF8.GetBytes(appSettings.JwtSetting.Encryptkey);

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretkey),
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidAudience = appSettings.JwtSetting.Audience,
                ValidateIssuer = true,
                ValidIssuer = appSettings.JwtSetting.Issuer,
                TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {                  
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();

                    if (context.Exception is SecurityTokenException or FormatException)
                        logger.LogWarning(context.Exception, "JWT authentication failed.");
                    else
                        logger.LogError(context.Exception, "Unexpected JWT authentication error.");

                    logger.LogError("Authentication failed.");

                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();

                    var principal = context.Principal;
                    var tokenUserId = principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                    var tokenSecurityStamp = principal?.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType)?.Value;
                    var tokenDeviceId = principal?.FindFirst("deviceId")?.Value;
                    var currentDeviceId = context.HttpContext.Request.Headers["deviceId"].ToString();

                    if (!long.TryParse(tokenUserId, out var userId) ||
                        tokenSecurityStamp.IsNullParameter() ||
                        tokenDeviceId.IsNullParameter())
                        throw new UnAuthorizedException("توکن نامعتبر است!");

                    if (string.IsNullOrWhiteSpace(currentDeviceId) ||
                        !string.Equals(tokenDeviceId, currentDeviceId, StringComparison.Ordinal))
                        throw new UnAuthorizedException("این توکن مطعلق به این دستگاه نیست!");

                    var uow = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
                    var userSecurityStamp = await uow.User.GetScurityStampAsync(userId, context.HttpContext.RequestAborted);
                    if (string.IsNullOrWhiteSpace(userSecurityStamp) ||
                        !string.Equals(userSecurityStamp, tokenSecurityStamp, StringComparison.Ordinal))
                        throw new UnAuthorizedException("توکن نامعتر است، لطفا دوباره لاگین کنید!");
                },
                OnChallenge = context =>
                {
                    throw new UnAuthorizedException();
                }
            };
        });

        return services;
    }
}
