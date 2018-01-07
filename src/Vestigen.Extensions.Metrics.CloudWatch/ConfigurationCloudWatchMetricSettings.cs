using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Vestigen.Extensions.Metrics.CloudWatch;

namespace Vestigen.Extensions.Metrics.CloudWatch
{
    public class ConfigurationCloudWatchMetricSettings : ICloudWatchMetricSettings
    {
        private readonly IConfiguration _configuration;

        public ConfigurationCloudWatchMetricSettings(IConfiguration configuration)
        {
            _configuration = configuration;
            ChangeToken = configuration.GetReloadToken();
        }

        public IChangeToken ChangeToken { get; private set; }

        public ICloudWatchMetricSettings Reload()
        {
            ChangeToken = null;
            return new ConfigurationCloudWatchMetricSettings(_configuration);
        }

        public string Prefix => _configuration["MetricPrefix"];

        public string Namespace => _configuration["MetricNamespace"];
    }
}
