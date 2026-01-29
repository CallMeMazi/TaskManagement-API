using System.Security.Claims;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Common.Classes;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Services.Halper;
public interface IJwtService
{
    GeneralResult<string> GenerateAccessToken(User user, string deviceId);
    GeneralResult<UserTokenDto> GenerateAccessTokenAndRefreshToken(User user, string deviceId);
    GeneralResult<string> GenerateRefreshToken();
    GeneralResult<string> GetClaimValueByAccessToken(string token, string claimType, string deviceId);
    GeneralResult<string> GetSecurityStampFromAccessToken(string token, string deviceId);
    GeneralResult<int> GetUserIdFromAccessToken(string token, string deviceId);
    GeneralResult<ClaimsPrincipal> ValidateAccessTokenAndGetPrincipal(string token, string deviceId);
}
