namespace MetroDigital.Application.Features.Basket
{
    public sealed class ArticleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
    }
}
