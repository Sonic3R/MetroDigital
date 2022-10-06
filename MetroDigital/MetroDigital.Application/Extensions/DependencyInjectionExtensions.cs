using FluentValidation;
using MediatR;
using MetroDigital.Application.Common.Behaviours;
using MetroDigital.Application.Common.Factories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MetroDigital.Application.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(typeof(DependencyInjectionExtensions).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddSingleton<ResponseFactory>();

            return services;
        }
    }
}
