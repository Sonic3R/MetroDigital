namespace MetroDigital.API.Models
{
    public sealed class BasketPostRequest
    {
        public BasketPostRequest(string customer, bool paysVat)
        {
            Customer = customer;
            PaysVat = paysVat;
        }

        public string Customer { get; }
        public bool PaysVat { get; }
    }

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

    public sealed class UpdateBasketRequest
    {
        public string Status { get; set; }
    }
}
