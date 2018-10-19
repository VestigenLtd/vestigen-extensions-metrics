using System;
using System.Collections.Generic;

namespace Vestigen.Extensions.Metrics.UnitTests.Abstractions
{
    public class TestSink : ITestSink
    {
        public TestSink(
            Func<TestReportingContext, bool> writeEnabled = null,
            Func<TestScopeContext, bool> beginEnabled = null)
        {
            WriteEnabled = writeEnabled;
            BeginEnabled = beginEnabled;

            ScopeContexts = new List<TestScopeContext>();
            ReportingContexts = new List<TestReportingContext>();
        }

        public Func<TestReportingContext, bool> WriteEnabled { get; set; }

        public Func<TestScopeContext, bool> BeginEnabled { get; set; }

        public List<TestScopeContext> ScopeContexts { get; set; }

        public List<TestReportingContext> ReportingContexts { get; set; }

        public void Write(TestReportingContext context)
        {
            if (WriteEnabled == null || WriteEnabled(context))
            {
                ReportingContexts.Add(context);
            }
        }

        public void Begin(TestScopeContext context)
        {
            if (BeginEnabled == null || BeginEnabled(context))
            {
                ScopeContexts.Add(context);
            }
        }

        public static bool EnableWithTypeName<T>(TestReportingContext context)
        {
            return context.MetricName.Equals(typeof(T).FullName);
        }

        public static bool EnableWithTypeName<T>(TestScopeContext context)
        {
            return context.LoggerName.Equals(typeof(T).FullName);
        }
    }
}
