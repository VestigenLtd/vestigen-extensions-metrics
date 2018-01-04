using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.Datadog
{
    public class DatadogMetricSettings : IDatadogMetricSettings
    {
        public IChangeToken ChangeToken { get; set; }

        public IDatadogMetricSettings Reload()
        {
            return this;
        }

        public string Prefix { get; set; }

        public string AgentAddress { get; set; }

        public int? AgentPort { get; set; }

        public int? AgentPacketSize { get; set; }
    }
}