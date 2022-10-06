using MediatR;
using MetroDigital.Application.Common.Base;
using MetroDigital.Application.Common.Factories;
using Microsoft.Extensions.Logging;

namespace MetroDigital.Application.Common.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : Request<TResponse>
        where TResponse : Response, new()
    {
        private readonly ILogger<TRequest> _logger;
        private readonly ResponseFactory _responseFactory;
        public UnhandledExceptionBehaviour(ILogger<TRequest> logger, ResponseFactory responseFactory)
        {
            _logger = logger;
            _responseFactory = responseFactory;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}", requestName, request);
                return _responseFactory.UnexpectedError<TResponse>(ex, _logger);
            }
        }
    }
}
