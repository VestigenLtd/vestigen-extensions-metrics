namespace Vestigen.Extensions.Metrics.Abstractions.Internal
{
    public class StopwatchFactory : IStopwatchFactory
    {
        public IStopwatch Get()
        {
            return new Stopwatch();
        }
    }
}