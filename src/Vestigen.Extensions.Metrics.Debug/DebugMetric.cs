using System;
using System.Text;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.Debug
{
    /// <inheritdoc />
    /// <summary>
    /// A metric that writes messages in the debug output window only when a debugger is attached.
    /// </summary>
    public class DebugMetric : IMetric
    {
        private readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugMetric"/> class.
        /// </summary>
        /// <param name="name">The name of the metric.</param>
        public DebugMetric(string name) 
            : this(new DebugMetricSettings
            {
                Prefix = name
            })
        {
        }
        
        public DebugMetric(IDebugMetricSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(settings.Prefix))
            {
                if (settings.Prefix == null)
                {
                    throw new ArgumentNullException(nameof(settings.Prefix));
                }
                throw new ArgumentException("Value cannot be empty", nameof(settings.Prefix));
            }

            _name = settings.Prefix;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return DebugMetricScope.Push(state.ToString());
        }

        public void Push<T>(MetricType type, string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            var metricBuilder = new StringBuilder();
            metricBuilder.Append(_name);
            metricBuilder.Append(DebugMetricScope.Current.CompleteName);
            metricBuilder.Append('.' + statName);
            
            System.Diagnostics.Debug.WriteLine($"{type}\t{metricBuilder}: {value}");
        }

        public void Counter<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Increment(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Decrement(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
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
