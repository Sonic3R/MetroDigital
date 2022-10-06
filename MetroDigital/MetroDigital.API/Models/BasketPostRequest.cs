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
}
