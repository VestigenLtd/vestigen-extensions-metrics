namespace Vestigen.Extensions.Metrics.Abstractions
{
    public interface IMetricScope
    {
        IMetricScope Parent { get; }

        string Name { get; }

        string CompleteName { get; }
    }
}
