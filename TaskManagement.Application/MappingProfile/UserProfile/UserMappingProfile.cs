using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.Features.User.Command.ChangePasswordUser;
using TaskManagement.Application.Features.User.Command.CreateUserCommand;
using TaskManagement.Application.Features.User.Command.DeleteUser;
using TaskManagement.Application.Features.User.Command.UpdateUserCommand;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.UserProfile;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // Command DTOs
        CreateMap<CreateUserAppDto, User>().ConstructUsing(src =>
        new User(
            src.MobileNumber,
            src.Email,
            src.Password,
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