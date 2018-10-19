using Microsoft.Extensions.Configuration;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.Datadog
{
    public static class DatadogMetricFactoryExtensions
    {
        /// <summary>
        /// Adds Datadog as a provider using a specific configuration.
        /// </summary>
        public static IMetricFactory AddDatadogMetrics(this IMetricFactory factory, IDatadogMetricSettings settings)
        {
            factory.AddProvider(new DatadogMetricProvider(settings));
            return factory;
        }

        /// <summary>
        /// Adds Datadog as a provider using a generic configuration.
        /// </summary>
        public static IMetricFactory AddDatadogMetrics(this IMetricFactory factory, IConfiguration configuration)
        {
            var settings = new ConfigurationDatadogMetricSettings(configuration);
            return factory.AddDatadogMetrics(settings);
        }
    }
}