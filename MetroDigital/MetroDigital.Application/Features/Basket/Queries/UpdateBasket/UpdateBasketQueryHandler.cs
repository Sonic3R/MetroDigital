using MetroDigital.Application.Common.Base;
using MetroDigital.Application.Common.Factories;
using MetroDigital.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MetroDigital.Application.Features.Basket.Queries.UpdateBasket
{
    public sealed class UpdateBasketQueryHandler : BaseRequestHandler<UpdateBasketQuery, UpdateBasketQueryResponse>
    {
        private readonly IMetroDigitalDbContextFactory _metroDigitalDbContextFactory;

        public UpdateBasketQueryHandler(ResponseFactory responseFactory, IMetroDigitalDbContextFactory metroDigitalDbContextFactory) : base(responseFactory)
        {
            _metroDigitalDbContextFactory = metroDigitalDbContextFactory;
        }

        public override async Task<UpdateBasketQueryResponse> Handle(UpdateBasketQuery request, CancellationToken cancellationToken)
        {
            using (var context = _metroDigitalDbContextFactory.Create())
            {
                var strategy = context.CreateExecutionStrategy();

                var basketFound = await context.Baskets.FirstOrDefaultAsync(c => c.BasketId == request.BasketId, cancellationToken);
                if (basketFound == null)
                {
                    return NotFound($"Basked ID {request.BasketId} was not found");
                }

                string? error = null;

                await strategy.ExecuteAsync(async (token) =>
                {
                    using (var transaction = await context.BeginTransactionAsync(token))
                    {
                        try
                        {
                            basketFound.Status = request.Status;
                            context.Baskets.Update(basketFound);

                            await context.SaveChangesAsync(token);
                            await transaction.CommitAsync(token);
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

                return Success(response => response.BasketItem = basketFound);
            }
        }
    }
}
