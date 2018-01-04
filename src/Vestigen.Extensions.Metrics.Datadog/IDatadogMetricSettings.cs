using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.Datadog
{
    public interface IDatadogMetricSettings
    {
        IChangeToken ChangeToken { get; }

        IDatadogMetricSettings Reload();

        string Prefix { get; }

        string AgentAddress { get; }

        int? AgentPort { get; }

        int? AgentPacketSize { get; }

    }
}