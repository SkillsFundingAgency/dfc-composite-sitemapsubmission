using DFC.Common.Standard.Logging;
using DFC.Composite.SiteMapSubmision.Functions;
using DFC.Composite.SiteMapSubmision.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DFC.Composite.SiteMapSubmision.Tests
{
    public class SiteMapNotifyTimerFunctionTests
    {
        private readonly BingSiteMapNotifyTimerFunction _bingSiteMapNotifyTimerFunction;
        private readonly Mock<ILogger<BingSiteMapNotifyTimerFunction>> _logger;
        private readonly Mock<ILoggerHelper> _loggerHelper;
        private readonly Mock<IPingService> _pingService;
        private readonly Mock<IConfiguration> _configuration;

        public SiteMapNotifyTimerFunctionTests()
        {
            _logger = new Mock<ILogger<BingSiteMapNotifyTimerFunction>>();
            _loggerHelper = new Mock<ILoggerHelper>();
            _pingService = new Mock<IPingService>();
            _configuration = new Mock<IConfiguration>();

            _bingSiteMapNotifyTimerFunction = new BingSiteMapNotifyTimerFunction(_logger.Object, _loggerHelper.Object, _pingService.Object, _configuration.Object);
        }

        [Fact]
        public void WhenInvoked_Calls_PingService()
        {
            var pingUrl = "http://somesearcheengine.com/ping=";
            var siteMapUrl = "http://somesite.com/sitemap";
            _configuration.Setup(x => x["BingPingUrl"]).Returns(pingUrl);
            _configuration.Setup(x => x["SiteMapUrl"]).Returns(siteMapUrl);

            _bingSiteMapNotifyTimerFunction.Run(null);

            _pingService.Verify(x => x.Ping(It.Is<string>(s => s == pingUrl + siteMapUrl)), Times.Once());
        }
    }
}
