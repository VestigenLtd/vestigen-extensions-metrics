using System.Collections.Generic;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.UnitTests.Abstractions
{
    public class TestMetricFactory : IMetricFactory
    {
        private readonly ITestSink _sink;
        private bool _disposed;
        private readonly List<IMetricProvider> _providers;

        public TestMetricFactory(ITestSink sink)
        {
            _sink = sink;
            _providers = new List<IMetricProvider>();
        }

        public IMetric CreateMetric(string name)
        {
            return new TestMetric(name, _sink, TestMetricThrowsExceptionAt.None, new List<string>() );
        }

        public void AddProvider(IMetricProvider provider)
        {
            _providers.Add(provider);
        }

        public List<IMetricProvider> GetProviders()
        {
            return _providers;
        }

        public bool CheckDisposed() => _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }
}
