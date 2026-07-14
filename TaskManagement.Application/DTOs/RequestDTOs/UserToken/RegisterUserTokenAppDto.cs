namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;

public record RegisterUserTokenAppDto(
    int UserId,
    string DeviceId,
    string UserIp,
    string UserAgent
);