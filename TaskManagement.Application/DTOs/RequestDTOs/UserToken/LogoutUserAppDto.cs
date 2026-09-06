namespace TaskManagement.Application.DTOs.RequestDTOs.UserToken;
public record LogoutUserAppDto(
    long UserId,
    string AccessToken,
    string DeviceId
);