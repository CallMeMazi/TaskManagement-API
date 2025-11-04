using System.Security.Claims;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Common.Classes;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Services.Halper;
public interface IJwtService
{
    GeneralResult<string?> GenerateAccessToken(User user, string currentIp, string currentAgent);
    GeneralResult<UserTokenDto?> GenerateAccessTokenAndRefreshToken(User user, string currentIp, string currentAgent);
    GeneralResult<string?> GenerateRefreshToken();
    GeneralResult<string?> GetClaimValueByAccessToken(string token, string claimType, string currentIp, string currentAgent);
    GeneralResult<string?> GetSecurityStampFromAccessToken(string token, string currentIp, string currentAgent);
    GeneralResult<int?> GetUserIdFromAccessToken(string token, string currentIp, string currentAgent);
    GeneralResult<ClaimsPrincipal?> ValidateAccessTokenAndGetPrincipal(string token, string currentIp, string currentAgent, bool getPrincipal = true);
}
