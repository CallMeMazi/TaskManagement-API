namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public record RevokeUserTokenAppDto(
    int UserId,
    string DeviceId
);