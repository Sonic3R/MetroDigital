using MetroDigital.API.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace MetroDigital.API.Swagger
{
    [ExcludeFromCodeCoverage]
    internal sealed class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var schema = GenerateSchemaByContext(context);
            operation.RequestBody = new OpenApiRequestBody { Required = schema != null };

            if (schema != null)
            {
                operation.RequestBody.Content.Add("application/json", new OpenApiMediaType
                {
                    Schema = schema,
                    Example = GetExampleByContext(context)
                });
            }
        }

        private static OpenApiSchema? GenerateSchemaByContext(OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod?.Equals("post", StringComparison.OrdinalIgnoreCase) == true)
            {
                if (context.ApiDescription.RelativePath?.Equals("baskets") == true)
                {
                    return context.SchemaGenerator.GenerateSchema(typeof(BasketPostRequest), context.SchemaRepository);
                }

                if (context.ApiDescription.RelativePath?.EndsWith("article-line") == true)
                {
                    return context.SchemaGenerator.GenerateSchema(typeof(ArticlePostRequest), context.SchemaRepository);
                }
            }

            if (context.ApiDescription.HttpMethod?.Equals("put", StringComparison.OrdinalIgnoreCase) == true && context.ApiDescription.RelativePath?.StartsWith("baskets") == true)
            {
                return context.SchemaGenerator.GenerateSchema(typeof(UpdateBasketRequest), context.SchemaRepository);
            }

            return null;
        }

        private static IOpenApiAny? GetExampleByContext(OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod?.Equals("post", StringComparison.OrdinalIgnoreCase) == true)
            {
                if (context.ApiDescription.RelativePath?.Equals("baskets") == true)
                {
                    return GetPostBasketRequestExample();
                }

                if (context.ApiDescription.RelativePath?.EndsWith("article-line") == true)
                {
                    return GetPostArticleRequestExample();
                }
            }

            if (context.ApiDescription.HttpMethod?.Equals("put", StringComparison.OrdinalIgnoreCase) == true && context.ApiDescription.RelativePath?.StartsWith("baskets") == true)
            {
                return GetUpdateRequestExample();
            }

            return null;
        }

        private static IOpenApiAny GetPostBasketRequestExample()
        {
            return new OpenApiObject
            {
                [nameof(BasketPostRequest.PaysVat)] = new OpenApiBoolean(true),
                [nameof(BasketPostRequest.Customer)] = new OpenApiString("customer name")
            };
        }

        private static IOpenApiAny GetPostArticleRequestExample()
        {
            return new OpenApiObject
            {
                [nameof(ArticlePostRequest.Article)] = new OpenApiString("article name"),
                [nameof(ArticlePostRequest.Price)] = new OpenApiDouble(10)
            };
        }

        private static IOpenApiAny GetUpdateRequestExample()
        {
            return new OpenApiObject
            {
                [nameof(UpdateBasketRequest.Status)] = new OpenApiString("status")
            };
        }
    }
}
