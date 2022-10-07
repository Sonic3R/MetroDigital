using FluentValidation;

namespace MetroDigital.Application.Features.Basket.Queries.AddBasket
{
    public class AddBasketQueryValidator : AbstractValidator<AddBasketQuery>
    {
        private const byte USER_MIN_LENGTH = 3;

        public AddBasketQueryValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("The user name is required");
            RuleFor(x => x.UserName).MinimumLength(USER_MIN_LENGTH).WithMessage($"Minimum length for user name is {USER_MIN_LENGTH}");
        }
    }
}
