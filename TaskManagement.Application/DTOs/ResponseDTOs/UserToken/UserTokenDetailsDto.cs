namespace TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
public record UserTokenDetailsDto(
    long Id,
    long UserId,
    string AccessTokenHash,
    string RefreshTokenHash,
    DateTime CreatedAt,
    DateTime LastUsedAt,
    string UserIp,
    string userAgent
);