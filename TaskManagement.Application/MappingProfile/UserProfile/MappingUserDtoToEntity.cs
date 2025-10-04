using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.UserProfile;
public class MappingUserDtoToEntity : Profile
{
    public MappingUserDtoToEntity()
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
    }
}
