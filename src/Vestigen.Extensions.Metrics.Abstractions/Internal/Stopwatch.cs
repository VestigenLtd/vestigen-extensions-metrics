namespace Vestigen.Extensions.Metrics.Abstractions.Internal
{
    /// <summary>
    /// Implements the stopwatch interface using the diagnostics stopwatch
    /// </summary>
    public class Stopwatch : IStopwatch
    {
        private readonly System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public long ElapsedMilliseconds()
        {
            return _stopwatch.ElapsedMilliseconds;
        }
    }
}
