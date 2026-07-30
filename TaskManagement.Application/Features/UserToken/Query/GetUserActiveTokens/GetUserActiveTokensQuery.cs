using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Query.GetUserActiveTokens;
public record GetUserActiveTokensQuery(int UserId)
    : IRequest<GeneralResult<List<UserTokenDetailsDto>>>;
