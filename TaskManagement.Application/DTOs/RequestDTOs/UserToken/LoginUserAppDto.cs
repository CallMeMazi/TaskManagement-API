namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public record LoginUserAppDto(
    string MobileNumber,
    string Password,
    string DeviceId,
    string UserIp,
    string UserAgent
);