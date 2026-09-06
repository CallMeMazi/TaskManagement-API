namespace TaskManagement.Application.DTOs.InternalDTOs.UserToken;
public record GenerateTokensInternalDto(
    long UserId,
    string MobileNumber,
    string SecurityStamp,
    string DeviceId
);
