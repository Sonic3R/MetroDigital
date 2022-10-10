namespace MetroDigital.Application.Features.Basket
{
    internal static class BasketMapper
    {
        public static IEnumerable<BasketDto?> Map(IEnumerable<Domain.Entities.Basket> baskets)
        {
            foreach (var basket in baskets)
            {
                yield return Map(basket);
            }
        }

        public static BasketDto? Map(Domain.Entities.Basket? basket)
        {
            if (basket == null)
            {
                return null;
            }

            return new BasketDto
            {
                Id = basket.BasketId,
                Status = basket.Status,
                Customer = basket.User.Name,
                PaysVAT = basket.PaysVAT,
                Articles = ArticleMapper.Map(basket.Articles)
            };
        }
    }
}
