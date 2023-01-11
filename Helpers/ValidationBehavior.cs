#region

using FluentValidation;
using FluentValidation.Results;
using MediatR;

#endregion

namespace TodoSample.Helpers;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();
        var context = new ValidationContext<TRequest>(request);
        var validationErrors = await Task.WhenAll(_validators.Select(t => t.ValidateAsync(context)));
        var errorsDictionary = validationErrors
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);
        if (errorsDictionary.Any())
        {
            var failures = new List<ValidationFailure>();
            foreach (var error in errorsDictionary)
            foreach (var failure in error.Value)
                failures.Add(new ValidationFailure(error.Key, failure));

            throw new ValidationException(failures);
        }

        return await next();
    }
}