using System;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    /// <summary>
    /// Represents a type used to report metrics.
    /// </summary>
    /// <remarks>Aggregates most metric patterns to a single method.</remarks>
    public interface IMetric
    {
        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        IDisposable BeginScope<TState>(TState state);

        void Push<T>(MetricType type, string statName, T value, double sampleRate, string[] tags = null);

        void Counter<T>(string statName, T value, double sampleRate = 1.0, string[] tags = null);

        void Increment(string statName, int value = 1, double sampleRate = 1.0, string[] tags = null);

        void Decrement(string statName, int value = 1, double sampleRate = 1.0, string[] tags = null);

        void Gauge<T>(string statName, T value, double sampleRate = 1.0, string[] tags = null);

        void Histogram<T>(string statName, T value, double sampleRate = 1.0, string[] tags = null);

        void Set<T>(string statName, T value, double sampleRate = 1.0, string[] tags = null);

        void Timer<T>(string statName, T value, double sampleRate = 1.0, string[] tags = null);

        IMetricTimer StartTimer(string name, double sampleRate = 1.0, string[] tags = null);

        void Time(Action action, string statName, double sampleRate = 1.0, string[] tags = null);

        T Time<T>(Func<T> func, string statName, double sampleRate = 1.0, string[] tags = null);
    }
}
