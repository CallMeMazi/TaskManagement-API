using TaskManagement.Application.Interfaces.Repositories;

namespace TaskManagement.Application.Interfaces.UnitOfWork;
public interface IUnitOfWork
{
    IOrganizationRepository OrganizationRepository { get; }
    IProjectRepository ProjectRepository { get; }
    ITaskInfoRepository TaskInfoRepository { get; }
    ITaskRepository TaskRepository { get; }
    IUserRepository UserRepository { get; }
    IUserTokenRepository UserTokenRepository { get; }

    void Save();
    void Save(bool acceptAllChangesOnSuccess);
    Task SaveAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
}
