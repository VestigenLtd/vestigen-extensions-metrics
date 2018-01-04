using System;
using System.Collections.Concurrent;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.NewRelic
{
    public class NewRelicMetricProvider : IMetricProvider
    {
        private readonly ConcurrentDictionary<string, NewRelicMetric> _metrics = new ConcurrentDictionary<string, NewRelicMetric>();

        private INewRelicMetricSettings _settings;

        public NewRelicMetricProvider(INewRelicMetricSettings settings)
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

        private NewRelicMetric CreateMetricImplementation(string name)
        {
            return new NewRelicMetric(name);
        }

        public void Dispose()
        {
        }
    }
}