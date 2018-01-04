using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.Debug
{
    /// <summary>
    /// Extension methods for the <see cref="IMetricFactory"/> class.
    /// </summary>
    public static class DebugMetricFactoryExtensions
    {
        /// <summary>
        /// Adds a debug metric that outputs value to the debugger output.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        public static IMetricFactory AddDebug(this IMetricFactory factory)
        {
            factory.AddProvider(new DebugMetricProvider());
            return factory;
        }
    }
}