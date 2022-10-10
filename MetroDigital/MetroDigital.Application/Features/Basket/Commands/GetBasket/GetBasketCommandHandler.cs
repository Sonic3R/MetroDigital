using MetroDigital.Application.Common.Base;
using MetroDigital.Application.Common.Factories;
using MetroDigital.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MetroDigital.Application.Features.Basket.Commands.GetBasket
{
    public sealed class GetBasketCommandHandler : BaseRequestHandler<GetBasketCommand, GetBasketCommandResponse>
    {
        private readonly IMetroDigitalDbContextFactory _metroDigitalDbContextFactory;

        public GetBasketCommandHandler(ResponseFactory responseFactory, IMetroDigitalDbContextFactory metroDigitalDbContextFactory)
            : base(responseFactory)
        {
            _metroDigitalDbContextFactory = metroDigitalDbContextFactory;
        }

        public override async Task<GetBasketCommandResponse> Handle(GetBasketCommand request, CancellationToken cancellationToken)
        {
            using (var context = _metroDigitalDbContextFactory.Create())
            {
                var basketItem = await context.Baskets
                                    .Include(c => c.User)
                                    .Include(c => c.Articles)
                                    .FirstOrDefaultAsync(x => x.BasketId == request.BasketId, cancellationToken);

                if (basketItem == null)
                {
                    return NotFound($"Basket item with ID: {request.BasketId} is not found.");
                }

                return Success(response => response.Response = BasketMapper.Map(basketItem));
            }
        }
    }
}
