using System;
using System.Collections.Generic;

namespace Vestigen.Extensions.Metrics
{
    public class MetricDisposableScopeSet : IDisposable
    {
        private readonly List<IDisposable> _scopes;

        public MetricDisposableScopeSet()
        {
            _scopes = new List<IDisposable>();
        }

        public void AddScope(IDisposable disposable)
        {
            _scopes.Add(disposable);
        }

        public void Dispose()
        {
            foreach (var scope in _scopes)
            {
                scope.Dispose();
            }
        }
    }
}