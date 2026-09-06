namespace TaskManagement.Application.DTOs.RequestDTOs.UserToken;

public record RegisterUserTokenAppDto(
    long UserId,
    string DeviceId,
    string UserIp,
    string UserAgent
);