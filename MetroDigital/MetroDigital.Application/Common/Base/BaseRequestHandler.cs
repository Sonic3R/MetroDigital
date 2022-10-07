using MediatR;
using MetroDigital.Application.Common.Factories;
using Microsoft.Extensions.Logging;

namespace MetroDigital.Application.Common.Base
{
    public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : Request<TResponse>
        where TResponse : Response, new()
    {
        private readonly ResponseFactory _responseFactory;

        protected BaseRequestHandler(ResponseFactory responseFactory)
        {
            _responseFactory = responseFactory;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        protected TResponse Success()
        {
            return _responseFactory.Success<TResponse>();
        }

        protected TResponse Success(Action<TResponse> response)
        {
            return _responseFactory.Success(response);
        }

        protected TResponse BadRequest(Action<ValidationErrorBuilder<TRequest, TResponse>> errors)
        {
            return _responseFactory.ValidationFailure(errors);
        }

        protected TResponse BadRequest(ValidationErrorBuilder<TRequest, TResponse> errors)
        {
            return _responseFactory.ValidationFailure(errors);
        }

        protected TResponse BadRequest(Exception ex, ILogger logger)
        {
            return _responseFactory.UnexpectedError<TResponse>(ex, logger);
        }

        protected TResponse NotFound(string errorMessage)
        {
            return _responseFactory.NotFound<TResponse>(errorMessage);
        }
    }
}
