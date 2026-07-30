using AutoMapper;
using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.DTOs.ResponseDTOs.User;
using TaskManagement.Application.Features.User.Command.ChangePasswordUser;
using TaskManagement.Application.Features.User.Command.CreateUser;
using TaskManagement.Application.Features.User.Command.DeleteUser;
using TaskManagement.Application.Features.User.Command.UpdateUser;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.UserProfile;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // Command DTOs
        CreateMap<CreateUserAppDto, User>().ConstructUsing((src, context) =>
        new User(
            src.MobileNumber,
            src.Email,
            (string)context.Items["PasswordHash"],
            src.FirstName,
            src.LastName
        ));

        // Query DTOs
        CreateMap<User, UserDetailsDto>();

        // MediatR Mappping
        CreateMap<CreateUserCommand, CreateUserAppDto>();
        CreateMap<UpdateUserCommand, UpdateUserAppDto>();
        CreateMap<DeleteUserCommand, DeleteUserAppDto>();
        CreateMap<ChangePasswordUserCommand, ChangePasswordUserAppDto>();
    }
}