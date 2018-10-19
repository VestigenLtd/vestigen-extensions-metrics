using Microsoft.Extensions.Configuration;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.ApplicationInsights
{
    public static class ApplicationInsightsMetricFactoryExtensions
    {
        /// <summary>
        /// Adds ApplicationInsights as a provider using a specific configuration.
        /// </summary>
        public static IMetricFactory AddApplicationInsightsMetrics(this IMetricFactory factory, IApplicationInsightsMetricSettings settings)
        {
            factory.AddProvider(new ApplicationInsightsMetricProvider(settings));
            return factory;
        }

        /// <summary>
        /// Adds ApplicationInsights as a provider using a generic configuration.
        /// </summary>
        public static IMetricFactory AddApplicationInsightsMetrics(this IMetricFactory factory,  IConfiguration configuration)
        {
            var settings = new ConfigurationApplicationInsightsMetricSettings(configuration);
            return factory.AddApplicationInsightsMetrics(settings);
        }
    }
}