using DFC.Common.Standard.Logging;
using DFC.Composite.SiteMapSubmision.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DFC.Composite.SiteMapSubmision.Functions
{
    public class GoogleSiteMapNotifyTimerFunction
    {
        private readonly ILogger<GoogleSiteMapNotifyTimerFunction> _logger;
        private readonly ILoggerHelper _loggerHelper;
        private readonly IPingService _pingService;
        private readonly IConfiguration _configuration;

        public GoogleSiteMapNotifyTimerFunction(
            ILogger<GoogleSiteMapNotifyTimerFunction> logger,
            ILoggerHelper loggerHelper,
            IPingService pingService,
            IConfiguration configuration)
        {
            _logger = logger;
            _loggerHelper = loggerHelper;
            _pingService = pingService;
            _configuration = configuration;
        }

        [FunctionName("GoogleSiteMapNotifyTimerFunction")]
        public async Task Run([TimerTrigger("%GoogleTimerTrigger%")]TimerInfo myTimer)
        {
            _loggerHelper.LogMethodEnter(_logger);

            var googlePingUrl = _configuration["GooglePingUrl"];
            var siteMapUrl = _configuration["SiteMapUrl"];

            try
            {
                var pingResponse = await _pingService.Ping(googlePingUrl + siteMapUrl);
                if (pingResponse != null)
                {
                    var pingContentResponse = await pingResponse.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Received {pingResponse.StatusCode} from google with response {pingContentResponse}");
                }
                else
                {
                    _logger.LogInformation($"Received null from google");
                }
            }
            catch (Exception ex)
            {
                _loggerHelper.LogException(_logger, Guid.NewGuid(), ex);

            }
            finally
            {
                _loggerHelper.LogMethodEnter(_logger);
            }
        }
    }
}
