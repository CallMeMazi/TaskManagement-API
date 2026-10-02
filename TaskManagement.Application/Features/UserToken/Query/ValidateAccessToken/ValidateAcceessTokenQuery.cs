using MediatR;

namespace TaskManagement.Application.Features.UserToken.Query.ValidateAccessToken;
public record ValidateAcceessTokenQuery(string AccessToken, string DeviceId)
    : IRequest;