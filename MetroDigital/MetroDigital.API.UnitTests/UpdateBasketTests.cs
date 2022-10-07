using MetroDigital.API.Models;
using MetroDigital.Application.Features.Basket.Queries.UpdateBasket;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;

namespace MetroDigital.API.UnitTests
{
    public class UpdateBasketTests : CommonTests
    {
        [Fact]
        public async Task UpdateBasketReturnsSuccess()
        {
            var senderMock = CreateSender(GetUpdateBasketSuccessResponse());

            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                }));

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PutAsJsonAsync("/baskets/1", new UpdateBasketRequest { Status = "status" });

                Assert.NotNull(result);
                Assert.True(result.IsSuccessStatusCode);
                VerifySender<UpdateBasketQueryResponse>(senderMock, Times.Once());
            }
        }

        [Fact]
        public async Task UpdateBasketReturnsNotFound()
        {
            var senderMock = CreateSender(GetUpdateBasketNotFoundResponse());

            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                }));

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PutAsJsonAsync("/baskets/1", new UpdateBasketRequest { Status = "status" });

                Assert.NotNull(result);
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.NotFound);
                VerifySender<UpdateBasketQueryResponse>(senderMock, Times.Once());
            }
        }

        [Fact]
        public async Task UpdateBasketReturnsBadRequest()
        {
            var senderMock = CreateSender(GetUpdateBasketBadRequestResponse());

            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                }));

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PutAsJsonAsync("/baskets/1", new UpdateBasketRequest { Status = "status" });

                Assert.NotNull(result);
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.BadRequest);
                VerifySender<UpdateBasketQueryResponse>(senderMock, Times.Once());
            }
        }

        private static UpdateBasketQueryResponse GetUpdateBasketSuccessResponse()
        {
            return new UpdateBasketQueryResponse
            {
                BasketItem = new Domain.Entities.Basket { BasketId = 1 },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        private static UpdateBasketQueryResponse GetUpdateBasketNotFoundResponse()
        {
            return new UpdateBasketQueryResponse
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found"
            };
        }

        private static UpdateBasketQueryResponse GetUpdateBasketBadRequestResponse()
        {
            return new UpdateBasketQueryResponse
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ErrorMessage = "Bad Request"
            };
        }
    }
}