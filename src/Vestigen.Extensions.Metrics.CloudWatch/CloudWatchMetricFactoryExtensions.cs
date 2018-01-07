using Microsoft.Extensions.Configuration;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.CloudWatch
{
    public static class CloudWatchMetricFactoryExtensions
    {
        /// <summary>
        /// Adds CloudWatch as a provider using a specific configuration.
        /// </summary>
        public static IMetricFactory AddCloudWatchMetrics(this IMetricFactory factory, ICloudWatchMetricSettings settings)
        {
            factory.AddProvider(new CloudWatchMetricProvider(settings));
            return factory;
        }

        /// <summary>
        /// Adds CloudWatch as a provider using a generic configuration.
        /// </summary>
        public static IMetricFactory AddCloudWatchMetrics(this IMetricFactory factory,  IConfiguration configuration)
        {
            var settings = new ConfigurationCloudWatchMetricSettings(configuration);
            return factory.AddCloudWatchMetrics(settings);
        }
    }
}