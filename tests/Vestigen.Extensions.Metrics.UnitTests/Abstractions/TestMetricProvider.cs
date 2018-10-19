using System.Collections.Generic;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.UnitTests.Abstractions
{

    public class TestMetricProvider : IMetricProvider
    {
        private readonly string _providerName;
        private readonly TestMetricThrowsExceptionAt _throwExceptionAt;
        private readonly List<string> _store;

        public TestMetricProvider(string providerName, TestMetricThrowsExceptionAt throwExceptionAt, List<string> store)
        {
            _providerName = providerName;
            _throwExceptionAt = throwExceptionAt;
            _store = store;
        }

        public IMetric CreateMetric(string name)
        {
            return new TestMetric($"{_providerName}.{name}", new TestSink(), TestMetricThrowsExceptionAt.None, new List<string>() );
        }

        public void Dispose()
        {
        }
    }
}
