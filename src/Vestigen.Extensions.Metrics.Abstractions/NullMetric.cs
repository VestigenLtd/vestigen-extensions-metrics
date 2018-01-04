using System;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    public class NullMetric : IMetric
    {
        public static NullMetric Instance { get; } = new NullMetric();

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullDisposable.Instance;
        }

        public void Push<T>(MetricType type, string statName, T value, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Counter<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Increment(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Decrement(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Gauge<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Histogram<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Set<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Timer<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
        }

        public IMetricTimer StartTimer(string name, double sampleRate = 1, string[] tags = null)
        {
            return new NullMetricTimer();
        }

        public void Time(Action action, string statName, double sampleRate = 1, string[] tags = null)
        {
            action();
        }

        public T Time<T>(Func<T> func, string statName, double sampleRate = 1, string[] tags = null)
        {
            return func();
        }

        public void Dispose()
        {
        }
    }
}
