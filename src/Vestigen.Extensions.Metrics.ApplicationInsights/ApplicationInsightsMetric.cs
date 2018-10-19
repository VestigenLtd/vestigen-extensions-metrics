using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.ApplicationInsights
{
    public class ApplicationInsightsMetric : IMetric
    {
        private readonly TelemetryClient _service;
        private readonly string _prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudWatchMetric"/> class using explicit settings.
        /// </summary>
        /// <param name="prefix">The name of the metric.</param>
        public ApplicationInsightsMetric(string prefix)
            : this(prefix, new TelemetryClient(TelemetryConfiguration.Active))
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsMetric"/> class using a <see cref="TelemetryClient"/> instance.
        /// </summary>
        /// <param name="service">The pre-configured ApplicationInsights statistics service.</param>
        public ApplicationInsightsMetric(string prefix, TelemetryClient client)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                if (prefix == null)
                {
                    throw new ArgumentNullException(nameof(prefix));
                }
                throw new ArgumentException("Namespace must not be a non-whitespace value", nameof(prefix));
            }

            _prefix = prefix;
            _service = client ?? throw new ArgumentNullException(nameof(client));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return MetricScope.Push(state.ToString());
        }

        public void Push<T>(MetricType type, string statName, T value, double sampleRate, string[] tags)
        {
            var metricValue = double.Parse(value.ToString());

            switch (type)
            {
                case MetricType.Timer:
                case MetricType.Counter:
                case MetricType.Gauge:
                case MetricType.Histogram:
                case MetricType.Set:
                    Push(statName, metricValue, tags);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void Push(string statName, double value, IEnumerable<string> tags)
        {
            var metricBuilder = new StringBuilder();
            metricBuilder.Append(MetricScope.Current.CompleteName);
            metricBuilder.Append((metricBuilder.Length > 0 ? "." : string.Empty) + statName);
            
            var metric = new MetricTelemetry(metricBuilder.ToString(), value);
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    var pieces = tag.Split(':');
                    metric.Properties.Add(pieces[0], pieces[1]);
                }
            }

            _service.TrackMetric(metric);
        }

        public void Counter<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Increment(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
            Push(statName, value, tags);
        }

        public void Decrement(string statName, int value = 1, double sampleRate = 1, params string[] tags)
        {
            Push(statName, value, tags);
        }

        public void Gauge<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Gauge, statName, value, sampleRate, tags);
        }

        public void Histogram<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Histogram, statName, value, sampleRate, tags);
        }

        public void Set<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Set, statName, value, sampleRate, tags);
        }

        public void Timer<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Timer, statName, value, sampleRate, tags);
        }

        public IMetricTimer StartTimer(string name, double sampleRate = 1, string[] tags = null)
        {
            return new MetricTimer(this, name, sampleRate, tags);
        }

        public void Time(Action action, string statName, double sampleRate = 1, string[] tags = null)
        {
            using (StartTimer(statName, sampleRate, tags))
            {
                action();
            }
        }

        public T Time<T>(Func<T> func, string statName, double sampleRate = 1, string[] tags = null)
        {
            using (StartTimer(statName, sampleRate, tags))
            {
                return func();
            }
        }
    }
}
