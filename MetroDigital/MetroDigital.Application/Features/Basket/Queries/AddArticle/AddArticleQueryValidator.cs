using FluentValidation;

namespace MetroDigital.Application.Features.Basket.Queries.AddArticle
{
    public sealed class AddArticleQueryValidator : AbstractValidator<AddArticleQuery>
    {
        private const byte MIN_ARTICLE_NAME_LENGTH = 3;

        public AddArticleQueryValidator()
        {
            RuleFor(x => x.ArticleName).NotNull().NotEmpty().WithMessage("Article name is required");
            RuleFor(x => x.ArticleName).MinimumLength(MIN_ARTICLE_NAME_LENGTH).WithMessage($"Minimum length for article name is {MIN_ARTICLE_NAME_LENGTH}");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greather than 0");
            RuleFor(x => x.BasketId).GreaterThan(0).WithMessage("Basket id must be specified");
        }
    }
}
