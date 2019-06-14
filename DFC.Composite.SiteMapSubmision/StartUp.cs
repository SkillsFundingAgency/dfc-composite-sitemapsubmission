using DFC.Common.Standard.Logging;
using DFC.Composite.SiteMapSubmision;
using DFC.Composite.SiteMapSubmision.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

[assembly: WebJobsStartup(typeof(StartUp))]
namespace DFC.Composite.SiteMapSubmision
{
    public class StartUp : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var config = CreateConfiguration(builder);
            RegisterServices(builder.Services, config);
        }

        private void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IPingService, PingService>();
            services.AddTransient<ILoggerHelper, LoggerHelper>();
        }

        private IConfiguration CreateConfiguration(IWebJobsBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (descriptor?.ImplementationInstance is IConfigurationRoot configuration)
            {
                configurationBuilder.AddConfiguration(configuration);
            }

            return configurationBuilder.Build();
        }
    }
}