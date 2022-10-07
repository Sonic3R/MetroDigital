using MetroDigital.Application.Common.Base;

namespace MetroDigital.Application.Features.Basket.Commands.GetBasket
{
    public sealed class GetBasketCommand : Request<GetBasketCommandResponse>
    {
        public int BasketId { get; set; }
    }
}
