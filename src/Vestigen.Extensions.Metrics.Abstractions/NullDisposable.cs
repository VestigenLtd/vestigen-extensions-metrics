using System;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    public class NullDisposable : IDisposable
    {
        public static readonly NullDisposable Instance = new NullDisposable();

        public void Dispose()
        {
            // intentionally does nothing
        }
    }
}
