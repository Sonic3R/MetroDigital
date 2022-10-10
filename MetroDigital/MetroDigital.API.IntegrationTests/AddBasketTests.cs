using MetroDigital.API.Models;
using MetroDigital.Application.Features.Basket;
using MetroDigital.Application.Features.Basket.Queries.AddBasket;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MetroDigital.API.IntegrationTests
{
    public class AddBasketTests
    {
        [Fact]
        public async Task AddBasketReturnsSuccess()
        {
            using (var application = new MetroDigitalApplication())
            {
                using (var client = application.CreateClient())
                {
                    const string user = "cosmin";
                    const bool payVAT = true;

                    var request = new BasketPostRequest(user, payVAT);
                    var response = await client.PostAsJsonAsync("/baskets", request);

                    Assert.True(response.IsSuccessStatusCode);

                    var dto = JsonConvert.DeserializeObject<BasketDto>(await response.Content.ReadAsStringAsync());
                    Assert.NotNull(dto);
                    Assert.True(dto.Id == 2);
                    Assert.True(dto.PaysVAT == payVAT);
                    Assert.Equal(user, dto.Customer);
                }
            }
        }

        [Fact]
        public async Task AddBasketReturnsUserNotFound()
        {
            using (var application = new MetroDigitalApplication())
            {
                using (var client = application.CreateClient())
                {
                    const string user = "george";
                    const bool payVAT = true;

                    var request = new BasketPostRequest(user, payVAT);
                    var response = await client.PostAsJsonAsync("/baskets", request);

                    Assert.False(response.IsSuccessStatusCode);

                    var obj = JsonConvert.DeserializeObject<AddBasketQueryResponse>(await response.Content.ReadAsStringAsync());
                    Assert.NotNull(obj);
                    Assert.NotEmpty(obj.ErrorMessage);
                    Assert.True(obj.ErrorMessage?.IndexOf(user) > -1);
                }
            }
        }
    }
}
