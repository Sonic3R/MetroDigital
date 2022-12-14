using MetroDigital.API.Models;
using MetroDigital.Application.Features.Basket;
using MetroDigital.Application.Features.Basket.Queries.UpdateBasket;
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
            var app = GetApp(senderMock);

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
            var app = GetApp(senderMock);

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
            var app = GetApp(senderMock);

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

        [Fact]
        public async Task AddBasketReturnsBadRequestFromInvalidJsonInput()
        {
            var senderMock = CreateSender(GetUpdateBasketBadRequestResponse());
            var app = GetApp(senderMock);

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PostAsync("/baskets", new FormUrlEncodedContent(new Dictionary<string, string> { { "invalid", "1" } }));

                Assert.NotNull(result);
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.BadRequest);
                VerifySender<UpdateBasketQueryResponse>(senderMock, Times.Never());
            }
        }

        private static UpdateBasketQueryResponse GetUpdateBasketSuccessResponse()
        {
            return new UpdateBasketQueryResponse
            {
                BasketItem = new BasketDto { Id = 1 },
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