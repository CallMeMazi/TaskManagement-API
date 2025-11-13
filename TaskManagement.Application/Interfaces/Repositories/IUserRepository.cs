using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IUserRepository : IBaseRepository<User, UserDetailsDto>
{
}