namespace MetroDigital.Domain.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = null!;

        public User User { get; set; } = null!;
        public List<Article> Articles { get; set; } = null!;
    }
}
