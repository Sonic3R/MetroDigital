using MetroDigital.Application.Common.Base;
using MetroDigital.Application.Common.Factories;
using MetroDigital.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MetroDigital.Application.Features.Basket.Queries.AddBasket
{
    public sealed class AddBasketQueryHandler : BaseRequestHandler<AddBasketQuery, AddBasketQueryResponse>
    {
        private readonly IMetroDigitalDbContextFactory _metroDigitalDbContextFactory;

        public AddBasketQueryHandler(ResponseFactory responseFactory, IMetroDigitalDbContextFactory metroDigitalDbContextFactory) : base(responseFactory)
        {
            _metroDigitalDbContextFactory = metroDigitalDbContextFactory;
        }

        public override async Task<AddBasketQueryResponse> Handle(AddBasketQuery request, CancellationToken cancellationToken)
        {
            using (var context = _metroDigitalDbContextFactory.Create())
            {
                var strategy = context.CreateExecutionStrategy();

                var userFound = await context.Users.FirstOrDefaultAsync(c => c.Name.Equals(request.UserName), cancellationToken);
                if (userFound == null)
                {
                    return NotFound($"User name {request.UserName} not found");
                }

                Domain.Entities.Basket? basket = null;
                string? error = null;

                await strategy.ExecuteAsync(async (token) =>
                {
                    using (var transaction = await context.BeginTransactionAsync(token))
                    {
                        try
                        {
                            var createdBasket = await context.Baskets.AddAsync(new Domain.Entities.Basket { PaysVAT = request.PaysVAT, UserId = userFound.UserId }, token);
                            await context.SaveChangesAsync(token);
                            await transaction.CommitAsync(token);

                            if (createdBasket.Entity != null)
                            {
                                basket = await context.Baskets.FirstAsync(x => x.BasketId == createdBasket.Entity.BasketId);
                            }
                        }
                        catch
                        {
                            await transaction.RollbackAsync(token);
                            error = "Basket item could not be saved";
                        }
                    }
                }, cancellationToken);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    return BadRequest(error);
                }

                return Success(response => response.BasketItem = BasketMapper.Map(basket));
            }
        }
    }
}
