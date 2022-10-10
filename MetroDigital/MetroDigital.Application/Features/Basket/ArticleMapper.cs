namespace MetroDigital.Application.Features.Basket
{
    internal static class ArticleMapper
    {
        public static IEnumerable<ArticleDto?> Map(IEnumerable<Domain.Entities.Article> articles)
        {
            foreach (var article in articles)
            {
                yield return Map(article);
            }
        }

        public static ArticleDto? Map(Domain.Entities.Article? article)
        {
            if (article == null)
            {
                return null;
            }

            return new ArticleDto
            {
                Id = article.ArticleId,
                Name = article.Name,
                Price = article.Price
            };
        }
    }
}
