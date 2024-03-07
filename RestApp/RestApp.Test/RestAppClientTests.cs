using System;
using System.Net;
using System.Threading.Tasks;
using Moq;
using RestApp.Logging;

namespace RestApp.Test
{
    public class RestAppClientTests
    {
        private Mock<IRestClient> _restClientMock;
        private Mock<ILogger> _loggerMock;
        private readonly int _retryCount = 3;
        private readonly int _delay = 1000;
        private const string Url = "https://restapi.com";

        private RestAppClient _restAppClient;

        [SetUp]
        public void Setup()
        {
            _restClientMock = new Mock<IRestClient>();
            _loggerMock = new Mock<ILogger>();

            _restAppClient = new RestAppClient(_restClientMock.Object, _loggerMock.Object, _retryCount, _delay);
        }

        [Test]
        public async Task ShouldReturnsResultOnFirstTry()
        {
            // Arrange
            var expected = new
            {
                statusCode = 200
            };
            _restClientMock
                .Setup(x => x.Get<object>(It.IsAny<string>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _restAppClient.Get<object>(Url);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
            _restClientMock.Verify(x => x.Get<object>(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ShouldBeSucceedOnLastTry()
        {
            // Arrange
            var expected = new
            {
                statusCode = 200
            };
            var callCount = 0;
            _restClientMock
                .Setup(x => x.Get<object>(It.IsAny<string>()))
                .ReturnsAsync(() =>
                {
                    callCount++;
                    if (callCount <= 3)
                    {
                        throw new WebException("Exception message");
                    }

                    return expected;
                });

            // Act
            var result = await _restAppClient.Get<object>(Url);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
            _restClientMock.Verify(x => x.Get<object>(It.IsAny<string>()), Times.Exactly(4));
            _loggerMock.Verify(x => x.Error(It.IsAny<Exception>()), Times.Never());
        }
        
        [Test]
        public void ShouldBeFailedAfterMaxRetries()
        {
            // Arrange
            var webException = new WebException("Exception message");
            _restClientMock
                .Setup(x => x.Get<object>(It.IsAny<string>()))
                .ThrowsAsync(webException);

            // Act
            Assert.ThrowsAsync<WebException>(async () => await _restAppClient.Get<object>(Url));

            // Assert
            _restClientMock.Verify(x => x.Get<object>(It.IsAny<string>()), Times.Exactly(4));
            _loggerMock.Verify(x => x.Error(webException), Times.Once);
        }

        [Test]
        public void ShouldFailOnNonWebException()
        {
            // Arrange
            _restClientMock
                .Setup(x => x.Get<object>(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("Argument exception message"));

            // Act
            Assert.ThrowsAsync<ArgumentException>(async () => await _restAppClient.Get<object>(Url));

            // Assert
            _restClientMock.Verify(x => x.Get<object>(It.IsAny<string>()), Times.Once);
        }
    }
}