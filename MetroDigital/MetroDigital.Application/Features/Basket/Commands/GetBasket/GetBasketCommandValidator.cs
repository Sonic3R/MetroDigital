using FluentValidation;

namespace MetroDigital.Application.Features.Basket.Commands.GetBasket
{
    public class GetBasketCommandValidator : AbstractValidator<GetBasketCommand>
    {
        public GetBasketCommandValidator()
        {
            RuleFor(x => x.BasketId).GreaterThan(0).WithMessage("Invalid ID");
        }
    }
}
