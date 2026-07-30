namespace TaskManagement.Application.DTOs.RequestDTOs.UserToken;
public record RefreshUserTokenAppDto(
    string RefreshToken,
    string DeviceId
);