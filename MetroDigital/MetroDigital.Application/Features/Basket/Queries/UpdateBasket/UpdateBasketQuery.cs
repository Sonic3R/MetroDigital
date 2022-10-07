using MetroDigital.Application.Common.Base;

namespace MetroDigital.Application.Features.Basket.Queries.UpdateBasket
{
    public sealed class UpdateBasketQuery : Request<UpdateBasketQueryResponse>
    {
        public int BasketId { get; set; }
        public string? Status { get; set; }
    }
}
