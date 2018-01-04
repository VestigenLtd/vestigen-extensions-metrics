using System;
using System.Collections.Generic;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics
{
    /// <inheritdoc />
    public class MetricFactory : IMetricFactory
    {
        private readonly Dictionary<string, Metric> _metrics = new Dictionary<string, Metric>(StringComparer.Ordinal);
        private readonly List<IMetricProvider> _providers = new List<IMetricProvider>();
        private readonly object _sync = new object();
        private volatile bool _disposed;

        public IMetric CreateMetric(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw categoryName == null
                    ? new ArgumentNullException(nameof(categoryName))
                    : new ArgumentException(nameof(categoryName));
            }

            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(MetricFactory));
            }

            Metric metric;
            lock (_sync)
            {
                if (!_metrics.TryGetValue(categoryName, out metric))
                {
                    metric = new Metric(this, categoryName);
                    _metrics[categoryName] = metric;
                }
            }
            return metric;
        }

        public void AddProvider(IMetricProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(MetricFactory));
            }

            lock (_sync)
            {
                _providers.Add(provider);
                foreach (var metric in _metrics)
                {
                    metric.Value.AddProvider(provider);
                }
            }
        }

        public List<IMetricProvider> GetProviders()
        {
            return _providers;
        }

        /// <summary>
        /// Check if the factory has been disposed.
        /// </summary>
        /// <returns>True when <see cref="Dispose()"/> as been called</returns>
        public bool IsDisposed => _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                foreach (var provider in _providers)
                {
                    try
                    {
                        provider.Dispose();
                    }
                    catch
                    {
                        // Swallow exceptions on dispose
                    }
                }
            }
        }
    }
}
