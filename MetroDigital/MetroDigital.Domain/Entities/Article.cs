namespace MetroDigital.Domain.Entities
{
    public sealed class Article
    {
        public int ArticleId { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }

        public int BaskedId { get; set; }
        public Basket Basket { get; set; } = null!;
    }
}
