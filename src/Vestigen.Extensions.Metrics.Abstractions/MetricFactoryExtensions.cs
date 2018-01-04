using System;
using Vestigen.Extensions.Metrics.Abstractions.Internal;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    /// <summary>
    /// IMetricFactory extension methods for common scenarios.
    /// </summary>
    public static class MetricFactoryExtensions
    {
        /// <summary>
        /// Creates a new IMetric instance using the full name of the given type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="factory">The factory.</param>
        public static IMetric<T> CreateMetric<T>(this IMetricFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            return new Metric<T>(factory);
        }
        /// <summary>
        /// Creates a new IMetric instance using the full name of the given type.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="type">The type.</param>
        public static IMetric CreateMetric(this IMetricFactory factory, Type type)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return factory.CreateMetric(TypeNameSimplifier.GetTypeDisplayName(type));
        }
    }
}
