using System;
using System.Linq;
using System.Text;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.NewRelic
{
    public class NewRelicMetric : IMetric
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewRelicMetric"/> class using explicit settings.
        /// </summary>
        /// <param name="prefix">The name of the metric.</param>
        public NewRelicMetric(string prefix)
            : this(new NewRelicMetricSettings
            {
                Prefix = prefix
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewRelicMetric"/> class using a settings class.
        /// </summary>
        /// <param name="settings">The settings class used to configure the metric</param>
        public NewRelicMetric(INewRelicMetricSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            
            if (settings.Prefix == null)
            {
                throw new ArgumentNullException(nameof(settings.Prefix));
            }
            
            global::NewRelic.Api.Agent.NewRelic.SetApplicationName(settings.Prefix);
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

            if (tags != null)
            {
                var items = tags.Select(x =>
                {
                    var pieces = x.Split(':');
                    return new {Key = pieces[0], Value = pieces[1]};
                });
                
                foreach (var item in items)
                {
                    global::NewRelic.Api.Agent.NewRelic.AddCustomParameter(item.Key, item.Value);
                }
            }

            switch (type)
            {
                case MetricType.Timer:
                    global::NewRelic.Api.Agent.NewRelic.RecordResponseTimeMetric($"Custom/{statName}", long.Parse(value.ToString()));
                    break;
                case MetricType.Counter:
                case MetricType.Gauge:
                case MetricType.Histogram:
                case MetricType.Set:
                    global::NewRelic.Api.Agent.NewRelic.RecordMetric($"Custom/{statName}", float.Parse(value.ToString()));
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
