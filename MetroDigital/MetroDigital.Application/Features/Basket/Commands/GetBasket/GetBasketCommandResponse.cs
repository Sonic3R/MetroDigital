using MetroDigital.Application.Common.Base;

namespace MetroDigital.Application.Features.Basket.Commands.GetBasket
{
    public sealed class GetBasketCommandResponse : Response
    {
        public MetroDigital.Domain.Entities.Basket Response { get; set; } = null!;
    }
}
