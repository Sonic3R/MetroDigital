using MetroDigital.Application.Common.Base;
using MetroDigital.Application.Common.Factories;
using MetroDigital.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MetroDigital.Application.Features.Basket.Queries.AddArticle
{
    public sealed class AddArticleQueryHandler : BaseRequestHandler<AddArticleQuery, AddArticleQueryResponse>
    {
        private readonly IMetroDigitalDbContextFactory _metroDigitalDbContextFactory;

        public AddArticleQueryHandler(ResponseFactory responseFactory, IMetroDigitalDbContextFactory metroDigitalDbContextFactory) : base(responseFactory)
        {
            _metroDigitalDbContextFactory = metroDigitalDbContextFactory;
        }

        public override async Task<AddArticleQueryResponse> Handle(AddArticleQuery request, CancellationToken cancellationToken)
        {
            using (var context = _metroDigitalDbContextFactory.Create())
            {
                var strategy = context.CreateExecutionStrategy();

                var basketFound = await context.Baskets.FirstOrDefaultAsync(c => c.BasketId == request.BasketId, cancellationToken);
                if (basketFound == null)
                {
                    return NotFound($"Basked ID {request.BasketId} was not found");
                }

                Domain.Entities.Article? article = null;
                string? error = null;

                await strategy.ExecuteAsync(async (token) =>
                {
                    using (var transaction = await context.BeginTransactionAsync(token))
                    {
                        try
                        {
                            var createdArticle = await context.Articles.AddAsync(new Domain.Entities.Article { BaskedId = basketFound.BasketId, Name = request.ArticleName, Price = request.Price }, token);

                            await context.SaveChangesAsync(token);
                            await transaction.CommitAsync(token);

                            if (createdArticle.Entity != null)
                            {
                                article = createdArticle.Entity;
                            }
                        }
                        catch
                        {
                            await transaction.RollbackAsync(token);
                            error = "Article item could not be saved";
                        }
                    }
                }, cancellationToken);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    return BadRequest(error);
                }

                return Success(response => response.ArticleItem = ArticleMapper.Map(article));
            }
        }
    }
}
