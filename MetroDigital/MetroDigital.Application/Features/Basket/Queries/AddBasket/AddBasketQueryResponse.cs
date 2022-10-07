using MetroDigital.Application.Common.Base;

namespace MetroDigital.Application.Features.Basket.Queries.AddBasket
{
    public sealed class AddBasketQueryResponse : Response
    {
        public MetroDigital.Domain.Entities.Basket? BasketItem { get; set; }
    }
}
