using AutoMapper;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class UserTokenRepository 
    : BaseRepository<UserToken, UserTokenDetailsDto>, IUserTokenRepository
{
    public UserTokenRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) { }
}
