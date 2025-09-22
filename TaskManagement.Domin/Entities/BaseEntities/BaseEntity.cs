using TaskManagement.Common.Exceptions;

namespace TaskManagement.Domin.Entities.BaseEntities;

public interface IBaseEntity
{
}

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; protected set; }
    public bool IsDelete { get; protected set; } = false;
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; protected set; }


    public virtual void SoftDelete()
    {
        if (IsDelete)
            throw new BadRequestException("این موجودیت از قبل حذف شده است!");

        IsDelete = true;
    }
}
