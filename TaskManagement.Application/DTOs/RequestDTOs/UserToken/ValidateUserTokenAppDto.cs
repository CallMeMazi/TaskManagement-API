namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public record ValidateUserTokenAppDto(
    string AccessToken,
    string DeviceId
);