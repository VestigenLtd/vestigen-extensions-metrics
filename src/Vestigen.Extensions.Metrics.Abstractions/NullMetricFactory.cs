using System.Collections.Generic;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    /// <inheritdoc />
    /// <summary>
    /// An <see cref="T:Vestigen.Extensions.Metrics.Abstractions.IMetricProvider" /> used to create instance of
    /// <see cref="T:Vestigen.Extensions.Metrics.Abstractions.NullMetric" /> that reports nothing.
    /// </summary>
    public class NullMetricFactory : IMetricFactory
    {
        public static readonly NullMetricFactory Instance = new NullMetricFactory();

        /// <inheritdoc />
        /// <remarks>
        /// This returns a <see cref="NullMetric"/> instance which reports nothing.
        /// </remarks>
        public IMetric CreateMetric(string name)
        {
            return NullMetric.Instance;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method ignores the parameter and does nothing.
        /// </remarks>
        public void AddProvider(IMetricProvider provider)
        {
        }

        public List<IMetricProvider> GetProviders()
        {
            return new List<IMetricProvider>();
        }

        public void Dispose()
        {
        }
    }
}
