using AutoMapper;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class UserRepository 
    : BaseRepository<User, UserDetailsDto>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) { }

}
