using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public interface IBaseConfiguration<T> : IEntityTypeConfiguration<T> 
    where T : class
{
}
