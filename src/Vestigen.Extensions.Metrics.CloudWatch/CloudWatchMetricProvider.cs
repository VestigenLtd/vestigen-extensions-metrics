using System;
using System.Collections.Concurrent;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.CloudWatch
{
    public class CloudWatchMetricProvider : IMetricProvider
    {
        private readonly ConcurrentDictionary<string, CloudWatchMetric> _metrics = new ConcurrentDictionary<string, CloudWatchMetric>();

        private ICloudWatchMetricSettings _settings;

        public CloudWatchMetricProvider(ICloudWatchMetricSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _settings.ChangeToken?.RegisterChangeCallback(OnConfigurationReload, null);
        }

        private void OnConfigurationReload(object state)
        {
            // The settings object needs to change as the old one is probably holding an old change token.
            _settings = _settings.Reload();

            // The token will change each time it reloads, so we need to register again.
            _settings?.ChangeToken?.RegisterChangeCallback(OnConfigurationReload, null);
        }

        public IMetric CreateMetric(string name)
        {
            return _metrics.GetOrAdd(name, CreateMetricImplementation);
        }

        private CloudWatchMetric CreateMetricImplementation(string name)
        {
            return new CloudWatchMetric(name);
        }

        public void Dispose()
        {
        }
    }
}