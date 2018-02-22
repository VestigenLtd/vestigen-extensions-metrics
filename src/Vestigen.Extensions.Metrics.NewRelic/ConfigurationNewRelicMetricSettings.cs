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

        public string Prefix => _configuration["Prefix"];
    }
}
