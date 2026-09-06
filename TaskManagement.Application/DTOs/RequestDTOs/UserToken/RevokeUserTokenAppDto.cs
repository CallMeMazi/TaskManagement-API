namespace TaskManagement.Application.DTOs.RequestDTOs.UserToken;
public record RevokeUserTokenAppDto(
    long UserId,
    string DeviceId
);