using FluentValidation.Results;
using MetroDigital.Application.Common.Base;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MetroDigital.Application.Common.Factories
{
    public sealed class ResponseFactory
    {
        private static TResponse CreateResponse<TResponse>(HttpStatusCode statusCode) where TResponse : Response
        {
            var r = Activator.CreateInstance<TResponse>();
            r.StatusCode = statusCode;
            r.IsSuccess = true;
            return r;
        }

        public TResponse Success<TResponse>() where TResponse : Response
        {
            return CreateResponse<TResponse>(HttpStatusCode.OK);
        }

        public TResponse Success<TResponse>(Action<TResponse> responseConfig) where TResponse : Response
        {
            var r = CreateResponse<TResponse>(HttpStatusCode.OK);
            responseConfig.Invoke(r);
            return r;
        }

        public TResponse UnexpectedError<TResponse>(Exception exception, ILogger logger) where TResponse : Response
        {
            var r = CreateResponse<TResponse>(HttpStatusCode.InternalServerError);
            r.IsSuccess = false;
            var errorId = $"Error_{Guid.NewGuid():n}";
            r.ErrorMessage = "An unexpected error has occured.\n" +
                             "Please try again.\n\n" +
                             $"If the problem persists contact support team.\nError ID: {errorId}";

            logger.LogError(exception, $"{errorId}", errorId, Environment.StackTrace);
            return r;
        }

        public TResponse ValidationFailure<TResponse>(List<ValidationFailure> validationFailures) where TResponse : Response
        {
            var r = CreateResponse<TResponse>(HttpStatusCode.BadRequest);
            r.IsSuccess = false;
            r.ValidationsErrors = validationFailures.Select(x => new ValidationError
            {
                PropertyName = x.PropertyName,
                ErrorMessage = x.ErrorMessage
            }).ToList();
            return r;
        }

        public TResponse ValidationFailure<TRequest, TResponse>(Action<ValidationErrorBuilder<TRequest, TResponse>> errors)
            where TRequest : Request<TResponse>
            where TResponse : Response, new()
        {
            var validationErrorFactory = new ValidationErrorBuilder<TRequest, TResponse>();
            errors.Invoke(validationErrorFactory);

            return ValidationFailure(validationErrorFactory);
        }

        public TResponse ValidationFailure<TRequest, TResponse>(ValidationErrorBuilder<TRequest, TResponse> errors)
            where TRequest : Request<TResponse>
            where TResponse : Response, new()
        {
            var validationErrors = errors.GetValidationFailures();
            var r = CreateResponse<TResponse>(HttpStatusCode.BadRequest);
            r.IsSuccess = false;
            r.ValidationsErrors = validationErrors;
            return r;
        }

        public TResponse NotFound<TResponse>(string errorMessage) where TResponse : Response
        {
            var r = CreateResponse<TResponse>(HttpStatusCode.NotFound);
            r.IsSuccess = false;
            r.ErrorMessage = errorMessage;
            return r;
        }
    }
}
