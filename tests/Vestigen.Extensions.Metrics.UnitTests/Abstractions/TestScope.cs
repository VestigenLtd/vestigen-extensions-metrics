using System;
using System.Threading;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.UnitTests.Abstractions
{
    public class TestScope : IMetricScope
    {
        private static readonly AsyncLocal<IMetricScope> Value = new AsyncLocal<IMetricScope>();
        
        internal TestScope(string name)
        {
            Name = name;
        }

        public IMetricScope Parent { get; private set; }

        public static IMetricScope Current
        {
            set => Value.Value = value;
            get => Value.Value;
        }
        public string Name { get; }
        public string CompleteName { get; }
        public static IDisposable Push(string name)
        {
            var temp = Current;
            Current = new TestScope(name)
            {
                Parent = temp
            };

            return new TestDisposableScope();
        }

        private class TestDisposableScope : IDisposable
        {
            public void Dispose()
            {
                Current = Current.Parent;
            }
        }
    }
}
