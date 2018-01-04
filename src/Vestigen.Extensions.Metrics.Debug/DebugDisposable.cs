using System;

namespace Vestigen.Extensions.Metrics.Debug
{
    public class DebugDisposable : IDisposable
    {
        public static readonly DebugDisposable Instance = new DebugDisposable();

        public void Dispose()
        {
        }
    }
}