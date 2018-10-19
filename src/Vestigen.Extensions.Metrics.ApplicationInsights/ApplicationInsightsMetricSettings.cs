using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.ApplicationInsights
{
    public class ApplicationInsightsMetricSettings : IApplicationInsightsMetricSettings
    {
        public IChangeToken ChangeToken { get; set; }

        public IApplicationInsightsMetricSettings Reload()
        {
            return this;
        }

        public string Prefix { get; set; }

        public string AgentAddress { get; set; }

        public int? AgentPort { get; set; }

        public int? AgentPacketSize { get; set; }
    }
}