using System.Net;

namespace MetroDigital.Application.Common.Base
{
    public class Response
    {
        public bool IsSuccess => StatusCode == HttpStatusCode.OK;
        public bool HasErrorMessage => !string.IsNullOrWhiteSpace(ErrorMessage);
        public string? ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool HasValidationErrors => ValidationsErrors.Any();
        public List<ValidationError> ValidationsErrors { get; set; } = new List<ValidationError>();
    }

    public class ValidationError
    {
        public string? PropertyName { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
