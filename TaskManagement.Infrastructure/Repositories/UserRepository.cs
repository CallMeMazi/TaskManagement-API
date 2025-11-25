using AutoMapper;
using Microsoft.EntityFrameworkCore;
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


    // Command methods
    public Task<int> SoftDeleteUserSpAsync(int userId, CancellationToken ct)
    {
        var query = string.Format("EXEC dbo.sp_SoftDeleteUser @UserId = {0}", userId);
        return _db.Database.ExecuteSqlRawAsync(query, ct);
    }

}
