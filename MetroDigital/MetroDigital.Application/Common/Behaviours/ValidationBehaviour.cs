using FluentValidation;
using MediatR;
using MetroDigital.Application.Common.Base;
using MetroDigital.Application.Common.Factories;

namespace MetroDigital.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : Request<TResponse>
         where TResponse : Response, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ResponseFactory _responseFactory;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ResponseFactory responseFactory)
        {
            _validators = validators;
            _responseFactory = responseFactory;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r.Errors.Count > 0)
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Count > 0)
                {
                    return _responseFactory.ValidationFailure<TResponse>(failures);
                }
            }

            return await next();
        }
    }
}
