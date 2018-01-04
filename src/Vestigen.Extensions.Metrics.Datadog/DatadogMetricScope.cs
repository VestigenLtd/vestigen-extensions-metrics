using System;
using System.Text;
using System.Threading;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.Datadog
{
    public class DatadogMetricScope : IMetricScope
    {
        private static readonly AsyncLocal<IMetricScope> Value = new AsyncLocal<IMetricScope>();

        internal DatadogMetricScope(string name)
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
            Current = new DatadogMetricScope(name)
            {
                Parent = temp
            };

            return new DebugMetricDisposableScope();
        }

        private class DebugMetricDisposableScope : IDisposable
        {
            public void Dispose()
            {
                Current = Current.Parent;
            }
        }
    }
}
