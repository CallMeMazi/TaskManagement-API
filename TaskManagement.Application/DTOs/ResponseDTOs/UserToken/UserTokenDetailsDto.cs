namespace TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
public record UserTokenDetailsDto(
    int Id,
    int UserId,
    string AccessTokenHash,
    string RefreshTokenHash,
    DateTime CreatedAt,
    DateTime LastUsedAt,
    string UserIp,
    string userAgent
);