using FluentValidation;

namespace MetroDigital.Application.Features.Basket.Queries.UpdateBasket
{
    public sealed class UpdateBasketQueryValidator : AbstractValidator<UpdateBasketQuery>
    {
        public UpdateBasketQueryValidator()
        {
            RuleFor(x => x.Status).NotNull().NotEmpty();
            RuleFor(x => x.BasketId).GreaterThan(0);
        }
    }
}
