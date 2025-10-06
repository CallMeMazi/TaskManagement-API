using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private bool disposedValue;

    public IUserRepository UserRepository { get; }
    public IOrganizationRepository OrganizationRepository { get; }
    public IProjectRepository ProjectRepository { get; }
    public ITaskRepository TaskRepository { get; }
    public ITaskInfoRepository TaskInfoRepository { get; }

    public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository, IOrganizationRepository organizationRepository
        , IProjectRepository projectRepository, ITaskRepository taskRepository, ITaskInfoRepository taskInfoRepository)
    {
        _context = context;
        UserRepository = userRepository;
        OrganizationRepository = organizationRepository;
        ProjectRepository = projectRepository;
        TaskRepository = taskRepository;
        TaskInfoRepository = taskInfoRepository;
    }

    public void Save()
        => _context.SaveChanges();
    public void Save(bool acceptAllChangesOnSuccess)
        => _context.SaveChanges(acceptAllChangesOnSuccess);
    public Task SaveAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
    public Task SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
}
