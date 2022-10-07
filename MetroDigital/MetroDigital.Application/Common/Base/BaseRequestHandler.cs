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

        protected TResponse Success(Action<TResponse> response)
        {
            return _responseFactory.Success(response);
        }

        protected TResponse BadRequest(string error, ILogger logger = null)
        {
            return _responseFactory.UnexpectedError<TResponse>(error, logger);
        }

        protected TResponse NotFound(string errorMessage)
        {
            return _responseFactory.NotFound<TResponse>(errorMessage);
        }
    }
}
