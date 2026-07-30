using System.Security.Claims;
using TaskManagement.Application.DTOs.InternalDTOs.UserToken;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Halper;
public interface IJwtService
{
    GeneralResult<string> GenerateAccessToken(GenerateTokensInternalDto tokenDto);
    GeneralResult<UserTokenDto> GenerateAccessTokenAndRefreshToken(GenerateTokensInternalDto tokenDto);
    GeneralResult<string> GenerateRefreshToken();
    GeneralResult<string> GetClaimValueByAccessToken(string token, string claimType, string deviceId);
    GeneralResult<string> GetSecurityStampFromAccessToken(string token, string deviceId);
    GeneralResult<int> GetUserIdFromAccessToken(string token, string deviceId);
    GeneralResult<ClaimsPrincipal> ValidateAccessTokenAndGetPrincipal(string token, string deviceId);
}
