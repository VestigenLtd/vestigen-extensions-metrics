using Vestigen.Extensions.Metrics;
using Vestigen.Extensions.Metrics.Abstractions;
using Vestigen.Extensions.Metrics.Debug;

namespace ConsoleApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Usage();
        }

        private static void Usage()
        {
            // Using the metric factory allows you to register multiple providers and publish to
            // all of them simultaneously. Useful in scenarios where you need to migrate between
            // platforms without making a hard cut-over between systems.
            
            var factory = new MetricFactory();
            //factory.AddProvider(new DatadogMetricProvider());
            //factory.AddProvider(new NewRelicMetricProvider());
            //factory.AddProvider(new CloudWatchMetricProvider());
            //factory.AddProvider(new PrometheusMetricProvider());
            //factory.AddProvider(new ApplicationInsightsMetricProvider());

            var usageByNameParameter = factory.CreateMetric("MetricFactory usage");
            var usageByTypeParameter = factory.CreateMetric<Program>();


            // In the event that you only have a single system you wish to interact with, you can
            // create metrics directly from the providers instead of going through the factory.
            var provider = new DebugMetricProvider();

            var usageByProvider = provider.CreateMetric("ProviderMetric");


            // When it comes to using the metrics that are created off of the factory or even
            // direct instantiation for a given provider, you have some pretty powerful options.

            var metric = factory.CreateMetric("ConsoleApp");

            using (metric.StartTimer("Timer"))
            {
                metric.Counter("Answer", 42);

                using (metric.BeginScope("OuterScope"))
                {
                    metric.Counter("DoSomething", 10);

                    using (metric.BeginScope("InnerScope"))
                    {
                        metric.Increment("DoWork");
                    }
                }
            }

            // The above will produce the follow metrics...

            // ConsoleApp.Answer = 42
            // ConsoleApp.OuterScope.DoSomething = 10
            // ConsoleApp.OuterScope.InnerScope.DoWork = 1;
            // ConsoleAPp.Timer = {milliseconds}
        }
    }
}
