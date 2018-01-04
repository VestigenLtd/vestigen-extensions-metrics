using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.Debug
{
    public interface IDebugMetricSettings
    {
        IChangeToken ChangeToken { get; }

        IDebugMetricSettings Reload();

        string Prefix { get; }
    }
}
