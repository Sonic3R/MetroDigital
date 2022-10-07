using MetroDigital.Application.Common.Base;

namespace MetroDigital.Application.Features.Basket.Queries.AddArticle
{
    public sealed class AddArticleQuery : Request<AddArticleQueryResponse>
    {
        public int BasketId { get; set; }

        public string? ArticleName { get; set; }

        public double Price { get; set; }
    }
}
