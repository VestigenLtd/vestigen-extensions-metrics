namespace Vestigen.Extensions.Metrics.Abstractions
{
    /// <inheritdoc />
    /// <summary>
    /// Provider for the <see cref="T:Vestigen.Extensions.Metrics.Abstractions.NullMetric" />.
    /// </summary>
    public class NullMetricProvider : IMetricProvider
    {
        public static NullMetricProvider Instance { get; } = new NullMetricProvider();

        /// <inheritdoc />
        public IMetric CreateMetric(string categoryName)
        {
            return NullMetric.Instance;
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
