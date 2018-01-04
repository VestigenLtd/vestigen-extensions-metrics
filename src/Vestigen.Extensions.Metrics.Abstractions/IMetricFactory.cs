using System;
using System.Collections.Generic;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a type used to configure the metric system and create instances of <see cref="T:Vestigen.Extensions.Metrics.Abstractions.IMetric" /> from
    /// the registered <see cref="T:Vestigen.Extensions.Metrics.Abstractions.IMetricProvider" />s.
    /// </summary>
    public interface IMetricFactory : IDisposable
    {
        /// <summary>
        /// Creates a new <see cref="IMetric"/> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the metric.</param>
        /// <returns>The <see cref="IMetric"/>.</returns>
        IMetric CreateMetric(string categoryName);

        /// <summary>
        /// Adds an <see cref="IMetricProvider"/> to the metric system.
        /// </summary>
        /// <param name="provider">The <see cref="IMetricProvider"/>.</param>
        void AddProvider(IMetricProvider provider);

        /// <summary>
        /// Returns a list of all providers registered with the factory.
        /// </summary>
        /// <returns></returns>
        List<IMetricProvider> GetProviders();
    }
}
