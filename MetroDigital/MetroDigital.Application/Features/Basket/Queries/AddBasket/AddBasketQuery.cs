using MetroDigital.Application.Common.Base;

namespace MetroDigital.Application.Features.Basket.Queries.AddBasket
{
    public sealed class AddBasketQuery : Request<AddBasketQueryResponse>
    {
        public string? UserName { get; set; }
        public bool PaysVAT { get; set; }
    }
}
