namespace Vestigen.Extensions.Metrics.Abstractions
{
    public class NullMetricTimer : IMetricTimer
    {
        public void Dispose()
        {
            // intentionally does nothing
        }

        public long ElapsedMilliseconds => 0;
    }
}
