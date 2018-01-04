using System;

namespace Vestigen.Extensions.Metrics.Abstractions
{
    /// <inheritdoc />
    /// <summary>
    /// An empty scope without any logic
    /// </summary>
    public class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
