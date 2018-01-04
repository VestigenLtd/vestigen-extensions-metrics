using System;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    public interface IMetricTimer : IDisposable
    {
        long ElapsedMilliseconds { get; }
    }
}
