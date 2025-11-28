using TaskManagement.Application.Interfaces.Repositories;

namespace TaskManagement.Application.Interfaces.UnitOfWork;
public interface IUnitOfWork
{
    IOrganizationMemberShipRepository OrganizationMemberShip { get; }
    IOrganizationRepository Organization { get; }
    IProjectRepository Project { get; }
    ITaskInfoRepository TaskInfo { get; }
    ITaskRepository Task { get; }
    IUserRepository User { get; }
    IUserTokenRepository UserToken { get; }
    IOrganizationInvitationRepository Invitation { get; }


    void Save();
    void Save(bool acceptAllChangesOnSuccess);
    Task SaveAsync(CancellationToken ct = default);
    Task SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken ct = default);
}
