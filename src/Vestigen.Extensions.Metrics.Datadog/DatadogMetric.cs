using System;
using System.Text;
using StatsdClient;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.Datadog
{
    public class DatadogMetric : IMetric
    {
        private readonly IDogStatsd _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatadogMetric"/> class using explicit settings.
        /// </summary>
        /// <param name="prefix">The name of the metric.</param>
        /// <param name="server">The agent hostname or address</param>
        /// <param name="port">The agent listening UDP port</param>
        /// <param name="packetSize">The agent packet size to use in communication</param>
        public DatadogMetric(string prefix, string server = "127.0.0.1", int? port = 8125, int? packetSize = 512)
            : this(new StatsdConfig
            {
                Prefix = prefix,
                StatsdServerName = server,
                StatsdPort = port.Value,
                StatsdMaxUDPPacketSize = packetSize.Value
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatadogMetric"/> class using a settings class.
        /// </summary>
        /// <param name="settings">The settings class used to configure the metric</param>
        public DatadogMetric(IDatadogMetricSettings settings)
            : this(new StatsdConfig
            {
                Prefix = settings.Prefix,
                StatsdServerName = settings.AgentAddress ?? "127.0.0.1",
                StatsdPort = settings.AgentPort ?? 8125,
                StatsdMaxUDPPacketSize = settings.AgentPacketSize ?? 512
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatadogMetric"/> class using a <see cref="DogStatsdService"/> instance.
        /// </summary>
        /// <param name="service">The pre-configured Datadog statistics service.</param>
        public DatadogMetric(IDogStatsd service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));          
        }

        private DatadogMetric(StatsdConfig configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration.Prefix))
            {
                if (configuration.Prefix == null)
                {
                    throw new ArgumentNullException(nameof(configuration.Prefix));
                }
                throw new ArgumentException("Value cannot be empty", nameof(configuration.Prefix));
            }
            
            _service = new DogStatsdService();
            _service.Configure(configuration);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return MetricScope.Push(state.ToString());
        }

        public void Push<T>(MetricType type, string statName, T value, double sampleRate, string[] tags)
        {
            var metricBuilder = new StringBuilder();
            metricBuilder.Append(MetricScope.Current.CompleteName);
            metricBuilder.Append((metricBuilder.Length > 0 ? "." : string.Empty) + statName);

            switch (type)
            {
                case MetricType.Timer:
                    _service.Timer(metricBuilder.ToString(), double.Parse(value.ToString()), sampleRate, tags);
                    break;
                case MetricType.Counter:
                    _service.Counter(metricBuilder.ToString(), double.Parse(value.ToString()), sampleRate, tags);
                    break;
                case MetricType.Gauge:
                    _service.Gauge(metricBuilder.ToString(), double.Parse(value.ToString()), sampleRate, tags);
                    break;
                case MetricType.Histogram:
                    _service.Histogram(metricBuilder.ToString(), double.Parse(value.ToString()), sampleRate, tags);
                    break;
                case MetricType.Set:
                    _service.Set(metricBuilder.ToString(), value, sampleRate, tags);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void Counter<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Increment(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Decrement(string statName, int value = 1, double sampleRate = 1, params string[] tags)
        {
            Push(MetricType.Counter, statName, value, sampleRate, tags);
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
