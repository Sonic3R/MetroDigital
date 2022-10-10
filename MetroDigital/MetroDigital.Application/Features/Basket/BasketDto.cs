namespace MetroDigital.Application.Features.Basket
{
    public sealed class BasketDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public bool PaysVAT { get; set; }
        public string Customer { get; set; } = null!;

        public IEnumerable<ArticleDto> Articles { get; set; } = null!;
    }
}
