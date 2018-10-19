using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.ApplicationInsights
{
    public interface IApplicationInsightsMetricSettings
    {
        IChangeToken ChangeToken { get; }

        IApplicationInsightsMetricSettings Reload();

        string Prefix { get; }
    }
}