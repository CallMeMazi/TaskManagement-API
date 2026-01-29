using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IUserRepository User { get; }
    public IUserTokenRepository UserToken { get; }
    public IOrganizationRepository Organization { get; }
    public IProjectRepository Project { get; }
    public ITaskRepository Task { get; }
    public ITaskInfoRepository TaskInfo { get; }
    public IOrganizationMemberShipRepository OrganizationMemberShip { get; }
    public IOrganizationInvitationRepository Invitation { get; }
    public IProjectMemberShipRepository ProjectMemberShip { get; }

    public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository, IOrganizationRepository organizationRepository
        , IProjectRepository projectRepository, ITaskRepository taskRepository, ITaskInfoRepository taskInfoRepository
        , IUserTokenRepository userTokenRepository, IOrganizationMemberShipRepository organizationMemberShipRepository
        , IOrganizationInvitationRepository invitation, IProjectMemberShipRepository projectMemberShip)
    {
        _context = context;
        User = userRepository;
        Organization = organizationRepository;
        Project = projectRepository;
        Task = taskRepository;
        TaskInfo = taskInfoRepository;
        UserToken = userTokenRepository;
        OrganizationMemberShip = organizationMemberShipRepository;
        Invitation = invitation;
        ProjectMemberShip = projectMemberShip;
    }

    public void Save()
    {
        _context.SaveChanges();
    }
    public void Save(bool acceptAllChangesOnSuccess)
    {
        _context.SaveChanges(acceptAllChangesOnSuccess);
    }
    public async Task SaveAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
    public async Task SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(acceptAllChangesOnSuccess, ct);
    }
}
