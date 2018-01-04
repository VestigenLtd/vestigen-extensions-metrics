using System;

namespace Vestigen.Extensions.Metrics.UnitTests.Abstractions
{
    public class TestDisposable : IDisposable
    {
        public static readonly TestDisposable Instance = new TestDisposable();

        public void Dispose()
        {
            // intentionally does nothing
        }
    }
}
