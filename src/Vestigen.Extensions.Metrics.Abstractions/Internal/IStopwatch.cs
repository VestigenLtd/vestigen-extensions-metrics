namespace Vestigen.Extensions.Metrics.Abstractions.Internal
{
    /// <summary>
    /// An interface in which to build our own stopwatch for testing purposes; also much more simplified than system's
    /// </summary>
    public interface IStopwatch
    {
        /// <summary>
        /// Start the stopwatch
        /// </summary>
        void Start();
        
        /// <summary>
        /// Stop the stopwatch
        /// </summary>
        void Stop();
        
        /// <summary>
        /// The total elapsed milliseconds between start and stop
        /// </summary>
        /// <returns></returns>
        long ElapsedMilliseconds();
    }
}
