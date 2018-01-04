using Microsoft.Extensions.Configuration;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.NewRelic
{
    public static class NewRelicMetricFactoryExtensions
    {
        /// <summary>
        /// Adds a datadog metric using a datadog specific configuration.
        /// </summary>
        public static IMetricFactory AddNewRelic(this IMetricFactory factory, INewRelicMetricSettings settings)
        {
            factory.AddProvider(new NewRelicMetricProvider(settings));
            return factory;
        }

        /// <summary>
        /// Adds a datadog metric using the generic configuration object.
        /// </summary>
        public static IMetricFactory AddNewRelic(this IMetricFactory factory,  IConfiguration configuration)
        {
            var settings = new ConfigurationNewRelicMetricSettings(configuration);
            return factory.AddNewRelic(settings);
        }
    }
}