using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.Debug
{
    /// <inheritdoc />
    /// <summary>
    /// The provider for the <see cref="T:Vestigen.Extensions.Metrics.Debug.DebugMetric" />.
    /// </summary>
    public class DebugMetricProvider : IMetricProvider
    {
        /// <inheritdoc /> 
        public IMetric CreateMetric(string name)
        {
            return new DebugMetric(name);
        }

        public void Dispose()
        {
        }
    }
}
