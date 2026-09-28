using FluentValidation;
using MediatR;
using TaskManagement.Application.Utilities.Exceptions;

namespace TaskManagement.Application.Pipelines;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!_validators.Any())
            return await next();

        var validationContext = new ValidationContext<TRequest>(request);

        var result = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(validationContext, ct))
        );

        var failures = result.SelectMany(vr => vr.Errors).Where(vf => vf != null).ToList();

        if (failures.Any())
            throw new ValidationFailureException("اطلاعات ورودی نامعتبر است!", failures.Select(vf => vf.ErrorMessage).ToList());

        return await next();
    }
}
