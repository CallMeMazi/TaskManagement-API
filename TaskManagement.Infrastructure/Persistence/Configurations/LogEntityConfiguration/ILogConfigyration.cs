using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Infrastructure.Persistence.Configurations.LogEntityConfiguration;
public interface ILogConfigyration<T> : IEntityTypeConfiguration<T>
    where T : class
{
}
