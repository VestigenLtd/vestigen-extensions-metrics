using Microsoft.Extensions.Primitives;
using Vestigen.Extensions.Metrics.Debug;

namespace Vestigen.Extensions.Metrics.Datadog
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