using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Query.ValidateAccessToken;
public record ValidateAcceessTokenQuery(string AccessToken, int DeviceId)
    : IRequest<GeneralResult>;