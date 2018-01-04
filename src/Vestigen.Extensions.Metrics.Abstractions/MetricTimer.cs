using Vestigen.Extensions.Metrics.Abstractions.Internal;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    public class MetricTimer : IMetricTimer
    {
        private bool _disposed;
        private readonly string _name;
        private readonly Stopwatch _stopWatch;
        private readonly double _sampleRate;
        private readonly string[] _tags;
        private readonly IMetric _metric;

        public MetricTimer(IMetric metric, string name, double sampleRate = 1.0, string[] tags = null)
        {
            _name = name;
            _metric = metric;
            _sampleRate = sampleRate;
            _tags = tags;
            
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public long ElapsedMilliseconds => _stopWatch.ElapsedMilliseconds();

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _stopWatch.Stop();
                _metric.Timer(_name, _stopWatch.ElapsedMilliseconds(), _sampleRate, _tags);
            }
        }
    }
}
