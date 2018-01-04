using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.NewRelic
{
    public class NewRelicMetricSettings : INewRelicMetricSettings
    {
        public IChangeToken ChangeToken { get; set; }

        public INewRelicMetricSettings Reload()
        {
            return this;
        }

        public string MetricPrefix { get; set; }
    }
}