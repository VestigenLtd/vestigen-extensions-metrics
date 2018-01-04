using System;
using System.Collections.Generic;

namespace Vestigen.Extensions.Metrics.UnitTests.Abstractions
{
    public interface ITestSink
    {
        Func<TestReportingContext, bool> WriteEnabled { get; set; }

        Func<TestScopeContext, bool> BeginEnabled { get; set; }

        List<TestScopeContext> ScopeContexts { get; set; }

        List<TestReportingContext> ReportingContexts { get; set; }

        void Write(TestReportingContext context);

        void Begin(TestScopeContext context);
    }
}
