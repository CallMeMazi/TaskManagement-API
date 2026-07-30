using AutoMapper;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Features.UserToken.Command.LoginUser;
using TaskManagement.Application.Features.UserToken.Command.LogoutUser;
using TaskManagement.Application.Features.UserToken.Command.RefreshUserToken;
using TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensExceptCurrentByUserId;
using TaskManagement.Application.Features.UserToken.Command.RevokeTokenByDeviceId;
using TaskManagement.Application.Features.UserToken.Query.ValidateAccessToken;

namespace TaskManagement.Application.MappingProfile.UserTokenProfile;
public class UserTokenMappingProfile : Profile
{
    public UserTokenMappingProfile()
    {
        // MediatR Mapping
        CreateMap<ValidateAcceessTokenQuery, ValidateUserTokenAppDto>();
        CreateMap<LoginUserCommand, LoginUserAppDto>();
        CreateMap<LogoutUserCommand, LogoutUserAppDto>();
        CreateMap<RefreshUserTokenCommand, RefreshUserTokenAppDto>();
        CreateMap<RevokeTokenByDeviceIdCommand, RevokeUserTokenAppDto>();
        CreateMap<RevokeAllTokensExceptCurrentByUserIdCommand, RevokeUserTokenAppDto>();
    }
}
