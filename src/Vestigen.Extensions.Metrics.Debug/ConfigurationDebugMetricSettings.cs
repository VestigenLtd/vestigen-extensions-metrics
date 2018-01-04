using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.Debug
{
    public class ConfigurationDebugMetricSettings : IDebugMetricSettings
    {
        private readonly IConfiguration _configuration;

        public ConfigurationDebugMetricSettings(IConfiguration configuration)
        {
            _configuration = configuration;
            ChangeToken = configuration.GetReloadToken();
        }

        public IChangeToken ChangeToken { get; private set; }

        public IDebugMetricSettings Reload()
        {
            ChangeToken = null;
            return new ConfigurationDebugMetricSettings(_configuration);
        }

        public string Prefix => _configuration["Prefix"];
    }
}
