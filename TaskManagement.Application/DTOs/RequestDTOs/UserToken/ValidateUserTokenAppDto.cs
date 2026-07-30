namespace TaskManagement.Application.DTOs.RequestDTOs.UserToken;
public record ValidateUserTokenAppDto(
    string AccessToken,
    string DeviceId
);