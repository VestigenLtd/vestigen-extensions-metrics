using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.Debug
{
    public class DebugMetricSettings : IDebugMetricSettings
    {
        public IChangeToken ChangeToken { get; set; }

        public IDebugMetricSettings Reload()
        {
            return this;
        }

        public string Prefix { get; set; }
    }
}