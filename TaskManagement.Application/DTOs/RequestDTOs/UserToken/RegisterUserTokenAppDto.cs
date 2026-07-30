namespace TaskManagement.Application.DTOs.RequestDTOs.UserToken;

public record RegisterUserTokenAppDto(
    int UserId,
    string DeviceId,
    string UserIp,
    string UserAgent
);