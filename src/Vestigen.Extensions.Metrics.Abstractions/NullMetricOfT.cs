using System;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    /// <inheritdoc />
    /// <summary>
    /// Minimalistic metric that does nothing.
    /// </summary>
    public class NullMetricOfT<TCategoryName> : IMetric<TCategoryName>
    {
        public static readonly NullMetricOfT<TCategoryName> Instance = new NullMetricOfT<TCategoryName>();

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public void Push<TTValue>(MetricType type, string statName, TTValue value, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Counter<T1>(string statName, T1 value, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Increment(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Decrement(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Gauge<T1>(string statName, T1 value, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Histogram<T1>(string statName, T1 value, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Set<T1>(string statName, T1 value, double sampleRate = 1, string[] tags = null)
        {
        }

        public void Timer<T1>(string statName, T1 value, double sampleRate = 1, string[] tags = null)
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

        public T1 Time<T1>(Func<T1> func, string statName, double sampleRate = 1, string[] tags = null)
        {
            return func();
        }
    }
}
