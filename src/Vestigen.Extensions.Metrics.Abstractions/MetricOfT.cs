using System;
using Vestigen.Extensions.Metrics.Abstractions.Internal;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    /// <inheritdoc />
    /// <summary>
    /// Delegates to a new <see cref="T:Vestigen.Extensions.Metrics.Abstractions.IMetric" /> instance using the full name of the given type, created by the
    /// provided <see cref="T:Vestigen.Extensions.Metrics.Abstractions.IMetricFactory" />.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public class Metric<T> : IMetric<T>
    {
        private readonly IMetric _metric;

        /// <summary>
        /// Creates a new <see cref="Metric{T}"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public Metric(IMetricFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _metric = factory.CreateMetric(TypeNameSimplifier.GetTypeDisplayName(typeof(T)));
        }

        IDisposable IMetric.BeginScope<TState>(TState state)
        {
            return _metric.BeginScope(state);
        }

        public void Push<T1>(MetricType type, string statName, T1 value, double sampleRate, string[] tags)
        {
            _metric.Push(type, statName, value, sampleRate, tags);
        }

        public void Counter<T1>(string statName, T1 value, double sampleRate = 1, string[] tags = null)
        {
            _metric.Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Increment(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
            _metric.Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Decrement(string statName, int value = 1, double sampleRate = 1, params string[] tags)
        {
            _metric.Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Gauge<T1>(string statName, T1 value, double sampleRate = 1, string[] tags = null)
        {
            _metric.Push(MetricType.Gauge, statName, value, sampleRate, tags);
        }

        public void Histogram<T1>(string statName, T1 value, double sampleRate = 1, string[] tags = null)
        {
            _metric.Push(MetricType.Histogram, statName, value, sampleRate, tags);
        }

        public void Set<T1>(string statName, T1 value, double sampleRate = 1, string[] tags = null)
        {
            _metric.Push(MetricType.Set, statName, value, sampleRate, tags);
        }

        public void Timer<T1>(string statName, T1 value, double sampleRate = 1, string[] tags = null)
        {
            _metric.Push(MetricType.Timer, statName, value, sampleRate, tags);
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

        public T1 Time<T1>(Func<T1> func, string statName, double sampleRate = 1, string[] tags = null)
        {
            using (StartTimer(statName, sampleRate, tags))
            {
                return func();
            }
        }
    }
}
