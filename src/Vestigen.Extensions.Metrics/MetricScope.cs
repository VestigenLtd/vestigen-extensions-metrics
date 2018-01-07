using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Vestigen.Extensions.Metrics.Abstractions;

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

    public class MetricScope : IMetricScope
    {
        private static readonly AsyncLocal<IMetricScope> Value = new AsyncLocal<IMetricScope>();

        public MetricScope(string name)
        {
            Name = name;
        }

        public IMetricScope Parent { get; private set; }

        public static IMetricScope Current
        {
            set => Value.Value = value;
            get => Value.Value ?? (Value.Value = new MetricScope(string.Empty));
        }

        public string Name { get; }

        public string CompleteName
        {
            get
            {
                var result = new StringBuilder();
                var current = Current;
                var length = result.Length;
                var name = string.Empty;

                while (current != null)
                {
                    name = $".{current}{name}";
                    current = current.Parent;
                }

                result.Insert(length, name.TrimStart('.'));

                return result.ToString();
            }
        }

        public static IDisposable Push(string name)
        {
            var temp = Current;
            Current = new MetricScope(name)
            {
                Parent = temp
            };

            return new MetricDisposableScope();
        }

        private class MetricDisposableScope : IDisposable
        {
            public void Dispose()
            {
                Current = Current.Parent;
            }
        }
    }
}
