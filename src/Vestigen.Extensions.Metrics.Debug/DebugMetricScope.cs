using System;
using System.Threading;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.Debug
{
    internal class DebugMetricScope : IMetricScope
    {
        private static readonly AsyncLocal<IMetricScope> Value = new AsyncLocal<IMetricScope>();

        private DebugMetricScope(string name)
        {
            Name = name;
        }

        public IMetricScope Parent { get; private set; }

        public static IMetricScope Current
        {
            private set => Value.Value = value;
            get => Value.Value;
        }

        public static IDisposable Push(string name)
        {
            var temp = Current;
            Current = new DebugMetricScope(name)
            {
                Parent = temp
            };

            return new DebugMetricDisposableScope();
        }

        public string Name { get; }

        public string CompleteName { get; }

        private class DebugMetricDisposableScope : IDisposable
        {
            public void Dispose()
            {
                Current = Current.Parent;
            }
        }
    }
}
