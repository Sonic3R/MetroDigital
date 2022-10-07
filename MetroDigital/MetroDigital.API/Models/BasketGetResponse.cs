using MetroDigital.Application.Common.Base;
using MetroDigital.Application.Features.Basket.Commands.GetBasket;
using MetroDigital.Domain.Entities;

namespace MetroDigital.API.Models
{
    public class BasketGetResponse
    {
        public BasketGetResponse() { }

        public BasketGetResponse(int id, IEnumerable<ArticleGetResponse> articles, double totalNet, double totalGross, string customer, bool paysVat)
        {
            Id = id;
            Articles = articles;
            TotalNet = totalNet;
            TotalGross = totalGross;
            Customer = customer;
            PaysVat = paysVat;
        }

        public int? Id { get; }
        public IEnumerable<ArticleGetResponse>? Articles { get; }
        public double? TotalNet { get; }
        public double? TotalGross { get; }
        public string? Customer { get; }
        public bool? PaysVat { get; }

        internal static BasketGetResponse MapFrom(Basket basketEntity, double vat = 0.1)
        {
            var articles = basketEntity.Articles.Select(s => new ArticleGetResponse(s.Name, s.Price));
            var total = articles.Sum(article => article.Price);
            var gross = total + (total * vat);

            return new BasketGetResponse(basketEntity.BasketId, articles, total, gross, basketEntity.User.Name, basketEntity.PaysVAT);
        }
    }

    public class BasketGetResponseError : BasketGetResponse
    {
        public BasketGetResponseError(List<ValidationError> errors, string errorMessage, int statusCode)
        {
            Errors = errors;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public List<ValidationError>? Errors { get; }
        public string ErrorMessage { get; }
        public int StatusCode { get; }

        internal static BasketGetResponseError MapFrom(GetBasketCommandResponse response)
        {
            return new BasketGetResponseError(response.ValidationsErrors, response.ErrorMessage, (int)response.StatusCode);
        }
    }

    public sealed class ArticleGetResponse
    {
        public ArticleGetResponse(string article, double price)
        {
            Article = article;
            Price = price;
        }

        public string Article { get; }
        public double Price { get; }
    }
}
