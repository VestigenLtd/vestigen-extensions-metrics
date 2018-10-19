using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.ApplicationInsights
{
    public class ConfigurationApplicationInsightsMetricSettings : IApplicationInsightsMetricSettings
    {
        private readonly IConfiguration _configuration;

        public ConfigurationApplicationInsightsMetricSettings(IConfiguration configuration)
        {
            _configuration = configuration;
            ChangeToken = configuration.GetReloadToken();
        }

        public IChangeToken ChangeToken { get; private set; }

        public IApplicationInsightsMetricSettings Reload()
        {
            ChangeToken = null;
            return new ConfigurationApplicationInsightsMetricSettings(_configuration);
        }

        public string Prefix => _configuration["MetricPrefix"];

        public string AgentAddress => _configuration["ServerAddress"];

        public int? AgentPort
        {
            get
            {
                const string configFileName = "ServerPort";
                const string configClassName = nameof(AgentPort);

                return ParseValue(configFileName, configClassName);
            }
        }

        public int? AgentPacketSize
        {
            get
            {
                const string configFileName = "ServerPacketSize";
                const string configClassName = nameof(AgentPacketSize);

                return ParseValue(configFileName, configClassName);
            }
        }

        private int? ParseValue(string configSetting, string configName)
        {
            var value = _configuration[configSetting];
            if (string.IsNullOrEmpty(value))
            {
                return new int?();
            }

            if (int.TryParse(value, out var valueParsed))
            {
                return valueParsed;
            }

            var message = $"Configuration value '{value}' for setting '{configName}' is not supported.";
            throw new InvalidOperationException(message);
        }
    }
}
