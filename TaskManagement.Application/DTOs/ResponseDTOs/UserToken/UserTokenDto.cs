namespace TaskManagement.Application.DTOs.SharedDTOs.UserToken;
public record UserTokenDto(
    string AccessTokenHash,
    string RefreshTokenHash
);