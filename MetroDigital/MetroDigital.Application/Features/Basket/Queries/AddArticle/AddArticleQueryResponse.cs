using MetroDigital.Application.Common.Base;
using MetroDigital.Domain.Entities;

namespace MetroDigital.Application.Features.Basket.Queries.AddArticle
{
    public sealed class AddArticleQueryResponse : Response
    {
        public Article? ArticleItem { get; set; }
    }
}
