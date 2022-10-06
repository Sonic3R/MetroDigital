using MetroDigital.API.Models;

namespace MetroDigital.API
{
    internal static class Endpoints
    {
        public static void SetupEndpoints(this WebApplication app)
        {
            app.MapPost("/baskets", async (HttpRequest request, CancellationToken cancellationToken) =>
            {
                var val = await request.ReadFromJsonAsync<BasketPostRequest>(cancellationToken);
            });

            app.MapPost("/baskets/{id}/article-line", async (HttpRequest request, int id, CancellationToken cancellationToken) =>
            {
                var val = await request.ReadFromJsonAsync<ArticlePostRequest>(cancellationToken);
            });

            app.MapGet("/baskets/{id}", async (int id, CancellationToken cancellationToken) =>
            {

            });

            app.MapPut("/baskets/{id}", async (HttpRequest request, int id, CancellationToken cancellationToken) =>
            {
                var val = await request.ReadFromJsonAsync<UpdateBasketRequest>(cancellationToken);
            });
        }
    }
}
