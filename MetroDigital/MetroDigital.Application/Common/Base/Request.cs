using MediatR;

namespace MetroDigital.Application.Common.Base
{
    public class Request<TResponse> : IRequest<TResponse> where TResponse : Response, new()
    {
    }
}
