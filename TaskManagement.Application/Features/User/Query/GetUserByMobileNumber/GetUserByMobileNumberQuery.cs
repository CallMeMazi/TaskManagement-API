using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.User;

namespace TaskManagement.Application.Features.User.Query.GetUserByMobileNumber;
public record GetUserByMobileNumberQuery(string MobileNumber)
    : IRequest<UserDetailsDto>;
