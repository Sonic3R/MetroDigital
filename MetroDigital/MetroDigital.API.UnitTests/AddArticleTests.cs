using MetroDigital.API.Models;
using MetroDigital.Application.Features.Basket.Queries.AddArticle;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;

namespace MetroDigital.API.UnitTests
{
    public class AddArticleTests : CommonTests
    {
        [Fact]
        public async Task AddArticleReturnsSuccess()
        {
            var senderMock = CreateSender(GetAddArticleSuccessResponse());

            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                }));

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PostAsJsonAsync("/baskets/1/article-line", new ArticlePostRequest("tomato", 60));

                Assert.NotNull(result);
                Assert.True(result.IsSuccessStatusCode);
                VerifySender<AddArticleQueryResponse>(senderMock, Times.Once());
            }
        }

        [Fact]
        public async Task AddArticleReturnsNotFound()
        {
            var senderMock = CreateSender(GetAddArticleNotFoundResponse());

            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                }));

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PostAsJsonAsync("/baskets/1/article-line", new ArticlePostRequest("tomato", 60));

                Assert.NotNull(result);
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.NotFound);
                VerifySender<AddArticleQueryResponse>(senderMock, Times.Once());
            }
        }

        [Fact]
        public async Task AddArticleReturnsBadRequest()
        {
            var senderMock = CreateSender(GetAddArticleBadRequestResponse());

            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                }));

            using (app)
            {
                var client = app.CreateClient();
                var result = await client.PostAsJsonAsync("/baskets/1/article-line", new ArticlePostRequest("tomato", 60));

                Assert.NotNull(result);
                Assert.False(result.IsSuccessStatusCode);
                Assert.True(result.StatusCode == System.Net.HttpStatusCode.BadRequest);
                VerifySender<AddArticleQueryResponse>(senderMock, Times.Once());
            }
        }

        private static AddArticleQueryResponse GetAddArticleSuccessResponse()
        {
            return new AddArticleQueryResponse
            {
                ArticleItem = new Domain.Entities.Article { ArticleId = 1 },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        private static AddArticleQueryResponse GetAddArticleNotFoundResponse()
        {
            return new AddArticleQueryResponse
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found"
            };
        }

        private static AddArticleQueryResponse GetAddArticleBadRequestResponse()
        {
            return new AddArticleQueryResponse
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ErrorMessage = "Bad Request"
            };
        }
    }
}