using MetroDigital.Application.Common.Base;

namespace MetroDigital.Application.Features.Basket.Queries.AddBasket
{
    public sealed class AddBasketQueryResponse : Response
    {
        public BasketDto? BasketItem { get; set; }
    }
}
