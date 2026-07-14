namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public record RefreshUserTokenAppDto(
    string RefreshToken,
    string DeviceId
);