namespace MetroDigital.API.Models
{
    public sealed class ArticlePostRequest
    {
        public ArticlePostRequest(string article, double price)
        {
            Article = article;
            Price = price;
        }

        public string Article { get; }
        public double Price { get; }
    }
}
