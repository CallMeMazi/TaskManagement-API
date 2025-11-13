using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IUserTokenRepository : IBaseRepository<UserToken, UserTokenDetailsDto>
{
}
