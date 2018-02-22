using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.CloudWatch
{
    public interface ICloudWatchMetricSettings
    {
        IChangeToken ChangeToken { get; }

        ICloudWatchMetricSettings Reload();

        string Prefix { get; }
    }
}