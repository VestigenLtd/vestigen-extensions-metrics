namespace Vestigen.Extensions.Metrics.Abstractions
{
    /// <inheritdoc />
    /// <summary>
    /// A generic interface for logging where the category name is derived from the specified
    /// <typeparamref name="TCategoryName" /> type name.
    /// Generally used to enable activation of a named <see cref="T:Vestigen.Extensions.Metrics.Abstractions.IMetric" /> from dependency injection.
    /// </summary>
    /// <typeparam name="TCategoryName">The type whose name is used for the metric category name.</typeparam>
    public interface IMetric<out TCategoryName> : IMetric
    {

    }
}
