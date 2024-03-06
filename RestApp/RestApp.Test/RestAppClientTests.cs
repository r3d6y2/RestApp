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

        private RestAppClient _restAppClient;
        
        [SetUp]
        public void Setup()
        {
            _restClientMock = new Mock<IRestClient>();
            _loggerMock = new Mock<ILogger>();

            _restAppClient = new RestAppClient(_restClientMock.Object, _loggerMock.Object, _retryCount, _delay);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}