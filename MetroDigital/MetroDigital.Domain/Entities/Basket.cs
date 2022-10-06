namespace MetroDigital.Domain.Entities
{
    public sealed class Basket
    {
        public int BasketId { get; set; }
        public string Status { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<Article> Articles { get; set; } = null!;
    }
}
