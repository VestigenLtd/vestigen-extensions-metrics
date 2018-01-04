using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.NewRelic
{
    public class ConfigurationNewRelicMetricSettings : INewRelicMetricSettings
    {
        private readonly IConfiguration _configuration;

        public ConfigurationNewRelicMetricSettings(IConfiguration configuration)
        {
            _configuration = configuration;
            ChangeToken = configuration.GetReloadToken();
        }

        public IChangeToken ChangeToken { get; private set; }

        public INewRelicMetricSettings Reload()
        {
            ChangeToken = null;
            return new ConfigurationNewRelicMetricSettings(_configuration);
        }

        public string MetricPrefix => _configuration["MetricPrefix"];

        public string ServerAddress => _configuration["ServerAddress"];

        public int? ServerPort
        {
            get
            {
                const string configFileName = "ServerPort";
                const string configClassName = nameof(ServerPort);

                return ParseValue(configFileName, configClassName);
            }
        }

        public int? ServerPacketSize
        {
            get
            {
                const string configFileName = "ServerPacketSize";
                const string configClassName = nameof(ServerPacketSize);

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

            if (int.TryParse(value, out var serverPort))
            {
                return serverPort;
            }

            var message = $"Configuration value '{value}' for setting '{configName}' is not supported.";
            throw new InvalidOperationException(message);
        }
    }
}
