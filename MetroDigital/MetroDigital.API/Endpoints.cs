using MediatR;
using MetroDigital.API.Models;
using MetroDigital.Application.Features.Basket.Commands.GetBasket;
using MetroDigital.Application.Features.Basket.Queries.AddBasket;
using System.Net;

namespace MetroDigital.API
{
    internal static class Endpoints
    {
        public static void SetupEndpoints(this WebApplication app)
        {
            app.MapPost("/baskets", async (HttpRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var val = await request.ReadFromJsonAsync<BasketPostRequest>(cancellationToken);
                var query = new AddBasketQuery { UserName = val.Customer, PaysVAT = val.PaysVat };
                var queryResponse = await sender.Send(query, cancellationToken);

                return GetResultByStatus(queryResponse.StatusCode, queryResponse);
            });

            app.MapPost("/baskets/{id}/article-line", async (HttpRequest request, int id, CancellationToken cancellationToken) =>
            {
                var val = await request.ReadFromJsonAsync<ArticlePostRequest>(cancellationToken);
            });

            app.MapGet("/baskets/{id}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new GetBasketCommand { BasketId = id };
                var response = await sender.Send(command, cancellationToken);
                var basketResponse = response.IsSuccess ?
                    BasketGetResponse.MapFrom(response.Response) :
                    BasketGetResponseError.MapFrom(response);

                return GetResultByStatus(response.StatusCode, basketResponse);
            });

            app.MapPut("/baskets/{id}", async (HttpRequest request, int id, CancellationToken cancellationToken) =>
            {
                var val = await request.ReadFromJsonAsync<UpdateBasketRequest>(cancellationToken);
            });
        }

        private static IResult GetResultByStatus(HttpStatusCode statusCode, object? data)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.InternalServerError:
                    return Results.BadRequest(data);

                case HttpStatusCode.NotFound:
                    return Results.NotFound(data);
            }

            return Results.Ok(data);
        }
    }
}
