namespace MetroDigital.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<Basket> Baskets { get; set; } = null!;
    }
}
