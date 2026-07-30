namespace TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
public record UserTokenDto(
    string AccessTokenHash,
    string RefreshTokenHash
);