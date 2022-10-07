using MediatR;
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
    }
}
