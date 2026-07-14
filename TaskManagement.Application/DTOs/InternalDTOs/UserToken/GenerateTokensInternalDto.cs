namespace TaskManagement.Application.DTOs.InternalDTOs.UserToken;
public record GenerateTokensInternalDto(
    int UserId,
    string MobileNumber,
    string SecurityStamp,
    string DeviceId
);
