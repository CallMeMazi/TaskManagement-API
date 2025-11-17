using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.UnitOfWork;
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

    public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository, IOrganizationRepository organizationRepository
        , IProjectRepository projectRepository, ITaskRepository taskRepository, ITaskInfoRepository taskInfoRepository
        , IUserTokenRepository userTokenRepository, IOrganizationMemberShipRepository organizationMemberShipRepository)
    {
        _context = context;
        User = userRepository;
        Organization = organizationRepository;
        Project = projectRepository;
        Task = taskRepository;
        TaskInfo = taskInfoRepository;
        UserToken = userTokenRepository;
        OrganizationMemberShip = organizationMemberShipRepository;
    }

    public void Save()
        => _context.SaveChanges();
    public void Save(bool acceptAllChangesOnSuccess)
        => _context.SaveChanges(acceptAllChangesOnSuccess);
    public Task SaveAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
    public Task SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken ct = default)
        => _context.SaveChangesAsync(acceptAllChangesOnSuccess, ct);
}
