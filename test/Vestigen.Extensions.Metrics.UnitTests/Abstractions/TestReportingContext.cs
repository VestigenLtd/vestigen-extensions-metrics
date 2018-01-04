using System;

namespace Vestigen.Extensions.Metrics.UnitTests.Abstractions
{
    public class TestReportingContext
    {
        //public LogLevel LogLevel { get; set; }

        //public EventId EventId { get; set; }

        public object State { get; set; }

        public Exception Exception { get; set; }

        public Func<object, Exception, string> Formatter { get; set; }

        public object Scope { get; set; }

        public string MetricName { get; set; }
    }
}
