namespace MetroDigital.Domain.Entities
{
    public sealed class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;

        public List<Basket> Baskets { get; set; } = null!;
    }
}
