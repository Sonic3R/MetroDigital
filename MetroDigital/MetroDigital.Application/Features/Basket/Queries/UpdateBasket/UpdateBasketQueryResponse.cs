using MetroDigital.Application.Common.Base;

namespace MetroDigital.Application.Features.Basket.Queries.UpdateBasket
{
    public sealed class UpdateBasketQueryResponse : Response
    {
        public BasketDto? BasketItem { get; set; }
    }
}
