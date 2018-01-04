using System;
using System.Collections.Generic;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics
{
    public class Metric : IMetric
    {
        private readonly string _name;
        private readonly List<IMetric> _metrics;

        public Metric(IMetricFactory metricFactory, string name)
        {
            if (metricFactory == null)
            {
                throw new ArgumentNullException(nameof(metricFactory));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw name == null
                    ? new ArgumentNullException(nameof(name))
                    : new ArgumentException(nameof(name));
            }

            _name = name;
            _metrics = new List<IMetric>();

            var providers = metricFactory.GetProviders();
            foreach (var provider in providers)
            {
                _metrics.Add(provider.CreateMetric(name));
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (_metrics == null)
            {
                return NullScope.Instance;
            }

            // If there is only a single provider, just be a simple pass through
            if (_metrics.Count == 1)
            {
                return _metrics[0].BeginScope(state);
            }

            // If there are more, then it's a bit more complicated
            var scope = new MetricDisposableScopeSet();
            List<Exception> exceptions = null;

            foreach (var metric in _metrics)
            {
                try
                {
                    var disposable = metric.BeginScope(state);
                    scope.AddScope(disposable);
                }
                catch (Exception ex)
                {
                    if (exceptions == null)
                    {
                        exceptions = new List<Exception>();
                    }

                    exceptions.Add(ex);
                }
            }

            if (exceptions != null && exceptions.Count > 0)
            {
                throw new AggregateException("An error occurred while attempting to report a metric", exceptions);
            }

            return scope;
        }

        public void Push<T>(MetricType type, string statName, T value, double sampleRate, string[] tags = null)
        {
            List<Exception> exceptions = null;
            foreach (var metric in _metrics)
            {
                try
                {
                    metric.Push(type, statName, value, sampleRate, tags);
                }
                catch (Exception ex)
                {
                    if (exceptions == null)
                    {
                        exceptions = new List<Exception>();
                    }
                    exceptions.Add(ex);
                }
            }

            if (exceptions != null && exceptions.Count > 0)
            {
                throw new AggregateException("An error occurred attepting to report metric", exceptions);
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

        internal void AddProvider(IMetricProvider metricProvider)
        {
            var metric = metricProvider.CreateMetric(_name);

            _metrics.Add(metric);
        }

        
    }
}
