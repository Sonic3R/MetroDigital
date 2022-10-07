using MetroDigital.API.Models;
using MetroDigital.Application.Features.Basket.Queries.AddArticle;
using MetroDigital.Application.Features.Basket.Queries.AddBasket;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;

namespace MetroDigital.API.UnitTests
{
    public class AddBasketTests : CommonTests
    {
        [Fact]
        public async Task AddBasketReturnsSuccess()
        {
            var senderMock = CreateSender(GetAddBasketSuccessResponse());

            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                }));

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PostAsJsonAsync("/baskets", new BasketPostRequest("customer", true));

                Assert.NotNull(result);
                Assert.True(result.IsSuccessStatusCode);
                VerifySender<AddBasketQueryResponse>(senderMock, Times.Once());
            }
        }

        [Fact]
        public async Task AddBasketReturnsNotFound()
        {
            var senderMock = CreateSender(GetAddBasketNotFoundResponse());

            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                }));

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PostAsJsonAsync("/baskets", new BasketPostRequest("customer", true));

                Assert.NotNull(result);
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.NotFound);
                VerifySender<AddBasketQueryResponse>(senderMock, Times.Once());
            }
        }

        [Fact]
        public async Task AddBasketReturnsBadRequest()
        {
            var senderMock = CreateSender(GetAddBasketBadRequestResponse());

            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                }));

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PostAsJsonAsync("/baskets", new BasketPostRequest("customer", true));

                Assert.NotNull(result);
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.BadRequest);
                VerifySender<AddBasketQueryResponse>(senderMock, Times.Once());
            }
        }

        [Fact]
        public async Task AddBasketReturnsBadRequestFromInvalidJsonInput()
        {
            var senderMock = CreateSender(GetAddBasketBadRequestResponse());

            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                }));

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PostAsync("/baskets", new FormUrlEncodedContent(new Dictionary<string, string> { { "invalid", "1" } }));

                Assert.NotNull(result);
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.BadRequest);
                VerifySender<AddBasketQueryResponse>(senderMock, Times.Never());
            }
        }

        private static AddBasketQueryResponse GetAddBasketSuccessResponse()
        {
            return new AddBasketQueryResponse
            {
                BasketItem = new Domain.Entities.Basket { BasketId = 1 },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        private static AddBasketQueryResponse GetAddBasketNotFoundResponse()
        {
            return new AddBasketQueryResponse
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found"
            };
        }

        private static AddBasketQueryResponse GetAddBasketBadRequestResponse()
        {
            return new AddBasketQueryResponse
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ErrorMessage = "Bad Request"
            };
        }
    }
}