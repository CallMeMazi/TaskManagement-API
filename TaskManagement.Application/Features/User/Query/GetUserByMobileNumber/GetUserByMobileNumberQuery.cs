using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.User;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Query.GetUserByMobileNumber;
public record GetUserByMobileNumberQuery(string MobileNumber)
    : IRequest<GeneralResult<UserDetailsDto>>;
