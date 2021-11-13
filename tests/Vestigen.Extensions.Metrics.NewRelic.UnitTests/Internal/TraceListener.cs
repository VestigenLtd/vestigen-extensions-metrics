using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Vestigen.Extensions.Metrics.NewRelic.UnitTests.Internal
{
    public class NewRelicTraceListener : TraceListener
    {
        private readonly List<string> _strings;
        private readonly StringBuilder _partial;
        
        public NewRelicTraceListener()
        {
            _strings = new List<string>();
            _partial = new StringBuilder();
        }
        
        public override void Write(string message)
        {
            _partial.Append(message);
        }

        public override void WriteLine(string message)
        {
            if (_partial.Length > 0)
            {
                _strings.Add(_partial.ToString());
                _partial.Clear();
            }
            _strings.Add(message);
        }

        public List<string> Strings
        {
            get
            {
                var test = new List<string>();
                test.AddRange(_strings);
                test.Add(_partial.ToString());
                return test;
            }
        }

        public void Clear()
        {
            _strings.Clear();
            _partial.Clear();
        }
    }
}