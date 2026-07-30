namespace TaskManagement.Application.DTOs.RequestDTOs.UserToken;
public record RevokeUserTokenAppDto(
    int UserId,
    string DeviceId
);