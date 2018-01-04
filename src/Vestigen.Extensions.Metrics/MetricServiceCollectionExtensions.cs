using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics
{
    /// <summary>
    /// Extension methods for setting up metrics services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class MetricServiceCollectionExtensions
    {
        /// <summary>
        /// Adds metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Singleton<IMetricFactory, MetricFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IMetric<>), typeof(Metric<>)));

            return services;
        }
    }
}