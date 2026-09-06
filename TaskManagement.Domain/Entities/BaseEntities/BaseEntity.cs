using TaskManagement.Common.Exceptions;

namespace TaskManagement.Domain.Entities.BaseEntities;

public interface IBaseEntity
{
}

public abstract class BaseEntity : IBaseEntity
{
    public long Id { get; protected set; }
    public bool IsDelete { get; protected set; } = false;
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// Assigns a generated id once. Rich model constructors stay free of infrastructure details.
    /// </summary>
    public void AssignId(long id)
    {
        if (Id != 0)
            throw new InvalidOperationException("Entity id is already assigned.");

        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Entity id must be a positive snowflake value.");

        Id = id;
    }

    public virtual void SoftDelete()
    {
        if (IsDelete)
            throw new BadRequestException("این موجودیت از قبل حذف شده است!");

        IsDelete = true;
    }
}
