using MetroDigital.Application.Common.Base;

namespace MetroDigital.Application.Features.Basket.Commands.GetBasket
{
    public sealed class GetBasketCommandResponse : Response
    {
        public BasketDto Response { get; set; } = null!;
    }
}
