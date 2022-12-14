using MediatR;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MetroDigital.API.UnitTests
{
    public abstract class CommonTests
    {
        protected static Mock<ISender> CreateSender<TResponse>(TResponse outputData)
        {
            var senderMock = new Mock<ISender>();
            senderMock.Setup(s => s.Send(It.IsAny<IRequest<TResponse>>(), It.IsAny<CancellationToken>())).ReturnsAsync(outputData);
            return senderMock;
        }

        protected static void VerifySender<TResponse>(Mock<ISender> senderMock, Times times)
        {
            senderMock.Verify(s => s.Send(It.IsAny<IRequest<TResponse>>(), It.IsAny<CancellationToken>()), times);
        }

        protected static WebApplicationFactory<Program> GetApp(Mock<ISender> senderMock)
        {
            return new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddScoped((_) => senderMock.Object);
                    services.Configure<JsonOptions>(opts =>
                    {
                        opts.SerializerOptions.IncludeFields = true;
                    });
                }));
        }
    }
}
