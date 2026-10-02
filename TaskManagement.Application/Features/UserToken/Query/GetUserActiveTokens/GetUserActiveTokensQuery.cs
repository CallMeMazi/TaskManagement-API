using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;

namespace TaskManagement.Application.Features.UserToken.Query.GetUserActiveTokens;
public record GetUserActiveTokensQuery(long UserId)
    : IRequest<List<UserTokenDetailsDto>>;
