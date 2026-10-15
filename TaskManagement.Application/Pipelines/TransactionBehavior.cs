using MediatR;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Application.Utilities.MarkerInterface;

namespace TaskManagement.Application.Pipelines;

public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ITransactionalCommand
{
    private readonly IUnitOfWork _uow;

    public TransactionBehavior(IUnitOfWork uow) => _uow = uow;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        await _uow.BeginTransactionAsync(ct);

        try
        {
            var result = await next(ct);

            await _uow.CommitTransactionAsync(ct);

            return result;
        }
        catch
        {
            await _uow.RollBackTransactionAsync(ct);

            throw;
        }
    }
}
