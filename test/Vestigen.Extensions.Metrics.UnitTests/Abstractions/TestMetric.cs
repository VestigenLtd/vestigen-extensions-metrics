using System;
using System.Collections.Generic;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.UnitTests.Abstractions
{
    public class TestMetric : IMetric
    {
        private object _scope;
        private readonly ITestSink _sink;
        private readonly string _name;
        private readonly TestMetricThrowsExceptionAt _throwExceptionAt;
        private readonly List<string> _store;

        public TestMetric(string name, ITestSink sink, TestMetricThrowsExceptionAt throwExceptionAt, List<string> store)
        {
            _sink = sink;
            _name = name;
            _throwExceptionAt = throwExceptionAt;
            _store = store;
        }

        public string Name { get; set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (_throwExceptionAt == TestMetricThrowsExceptionAt.BeginScope)
            {
                throw new InvalidOperationException($"{_name}-Error occurred while creating scope.");
            }
            _store.Add($"{_name}-{state}");

            _scope = state;

            _sink.Begin(new TestScopeContext
            {
                LoggerName = _name,
                Scope = state
            });

            return TestScope.Push(state.ToString());
        }

        public void Push<T>(MetricType type, string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            if (_throwExceptionAt == TestMetricThrowsExceptionAt.Push)
            {
                throw new InvalidOperationException($"{_name}-Error occurred while pushing data.");
            }
            _store.Add($"{_name}-{statName}");

            _sink.Write(new TestReportingContext
            {
                MetricName = _name,
                Scope = _scope
            });
        }

        public void Counter<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            throw new NotImplementedException();
        }

        public void Increment(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
            throw new NotImplementedException();
        }

        public void Decrement(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
            throw new NotImplementedException();
        }

        public void Gauge<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            throw new NotImplementedException();
        }

        public void Histogram<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            throw new NotImplementedException();
        }

        public void Timer<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            throw new NotImplementedException();
        }

        public IMetricTimer StartTimer(string name, double sampleRate = 1, string[] tags = null)
        {
            throw new NotImplementedException();
        }

        public void Time(Action action, string statName, double sampleRate = 1, string[] tags = null)
        {
            throw new NotImplementedException();
        }

        public T Time<T>(Func<T> func, string statName, double sampleRate = 1, string[] tags = null)
        {
            throw new NotImplementedException();
        }
    }
}
