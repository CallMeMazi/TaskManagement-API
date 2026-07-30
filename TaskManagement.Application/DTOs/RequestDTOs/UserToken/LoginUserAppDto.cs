namespace TaskManagement.Application.DTOs.RequestDTOs.UserToken;
public record LoginUserAppDto(
    string MobileNumber,
    string Password,
    string DeviceId,
    string UserIp,
    string UserAgent
);