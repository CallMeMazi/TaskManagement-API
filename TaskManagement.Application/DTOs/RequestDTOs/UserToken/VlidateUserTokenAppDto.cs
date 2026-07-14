namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public record VlidateUserTokenAppDto(
    string AccessToken,
    string DeviceId
);