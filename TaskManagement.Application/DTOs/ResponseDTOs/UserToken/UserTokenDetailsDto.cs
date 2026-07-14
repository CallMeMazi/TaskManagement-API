namespace TaskManagement.Application.DTOs.SharedDTOs.UserToken;
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