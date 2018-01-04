using System;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    /// <inheritdoc />
    public interface IMetricProvider : IDisposable
    {
        /// <summary>
        /// Creates a new <see cref="IMetric"/> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the metric.</param>
        /// <returns></returns>
        IMetric CreateMetric(string categoryName);
    }
}
