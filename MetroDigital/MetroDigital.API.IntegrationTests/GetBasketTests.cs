using MetroDigital.API.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MetroDigital.API.IntegrationTests
{
    public class GetBasketTests
    {
        [Fact]
        public async Task GetBasketByIdReturnsSuccess()
        {
            using (var application = new MetroDigitalApplication())
            {
                using (var client = application.CreateClient())
                {
                    var basketItem = await client.GetFromJsonAsync<BasketGetResponse>("/baskets/1");

                    Assert.NotNull(basketItem);
                    Assert.True(basketItem.Id == 1);
                    Assert.True(basketItem.Articles.Count() == 1);
                    Assert.True(basketItem.Customer.Equals("Andrei"));
                    Assert.True(basketItem.PaysVat);

                    var article = basketItem.Articles.First();
                    Assert.True(article.Article.Equals("tomato"));
                    Assert.True(article.Price == 10);
                }
            }
        }

        [Fact]
        public async Task GetBasketByIdReturnsNotFound()
        {
            using (var application = new MetroDigitalApplication())
            {
                using (var client = application.CreateClient())
                {
                    var response = await client.GetAsync("/baskets/100");
                    Assert.False(response.IsSuccessStatusCode);

                    var model = JsonConvert.DeserializeObject<BasketGetResponseError>(await response.Content.ReadAsStringAsync());
                    Assert.NotNull(model);
                    Assert.NotEmpty(model.ErrorMessage);
                }
            }
        }
    }
}