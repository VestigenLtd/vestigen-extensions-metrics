using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.NewRelic
{
    public interface INewRelicMetricSettings
    {
        IChangeToken ChangeToken { get; }

        INewRelicMetricSettings Reload();

        string Prefix { get; }
    }
}