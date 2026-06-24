using FluentValidation;
using MediatR;

namespace Ticketer.Application.Common.Behaviours;

// This pipeline behavior runs before EVERY command or query handled by MediatR
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    // MediatR will automatically inject all FluentValidators registered in the DI container
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            // Run all validators for this specific request type concurrently
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                // If any validation fails, we throw an exception here.
                // The command handler is completely skipped!
                throw new ValidationException(failures);
            }
        }

        // If validation passes, we call next() to proceed to the actual CommandHandler
        return await next();
    }
}
