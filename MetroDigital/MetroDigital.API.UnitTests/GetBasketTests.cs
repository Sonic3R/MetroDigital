using MetroDigital.API.Models;
using MetroDigital.Application.Features.Basket.Commands.GetBasket;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace MetroDigital.API.UnitTests
{
    public class GetBasketTests : CommonTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetBasketReturnsSuccess(int basketId)
        {
            var senderMock = CreateSender(GetUpdateBasketSuccessResponse(basketId));
            var app = GetApp(senderMock);

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.GetFromJsonAsync<BasketGetResponse>($"/baskets/{basketId}");

                Assert.NotNull(result);
                Assert.True(result.Id == basketId);
                VerifySender<GetBasketCommandResponse>(senderMock, Times.Once());
            }
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.BadRequest)]
        public async Task GetBasketReturnsCustomError(HttpStatusCode statusCode)
        {
            var senderMock = CreateSender(GetUpdateBasketErrorResponse((int)statusCode));
            var app = GetApp(senderMock);

            using (app)
            {
                var client = app.CreateClient();
                var response = await client.GetAsync("/baskets/1");
                Assert.NotNull(response);
                Assert.False(response.IsSuccessStatusCode);

                var result = JsonConvert.DeserializeObject<BasketGetResponseError>(await response.Content.ReadAsStringAsync());

                Assert.NotNull(result);
                Assert.Null(result?.Articles);
                Assert.True(result.StatusCode == (int)statusCode);
                VerifySender<GetBasketCommandResponse>(senderMock, Times.Once());
            }
        }

        private static GetBasketCommandResponse GetUpdateBasketSuccessResponse(int id)
        {
            return new GetBasketCommandResponse { Response = new Application.Features.Basket.BasketDto { Id = id }, StatusCode = HttpStatusCode.OK };
        }

        private static GetBasketCommandResponse GetUpdateBasketErrorResponse(int statusCode)
        {
            return new GetBasketCommandResponse { StatusCode = (HttpStatusCode)statusCode, ErrorMessage = $"error {statusCode}" };
        }
    }
}
