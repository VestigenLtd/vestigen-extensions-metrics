using System;
using System.Diagnostics;
using Moq;
using Vestigen.Extensions.Metrics.Abstractions;
using Vestigen.Extensions.Metrics.NewRelic.UnitTests.Internal;
using Xunit;

namespace Vestigen.Extensions.Metrics.NewRelic.UnitTests
{
    public class NewRelicMetricTest
    {
        [Fact]
        public void Constructor_WhenGivenNullPrefix_ThrowsArgumentNullException()
        {
            // Arrange
            var prefix = default(string);

            // Act
            Action capture = () =>
            {
                var sut = new NewRelicMetric(prefix);
            };

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }
        
        [Fact]
         public void Constructor_WhenGivenEmptyPrefix_ThrowsArgumentException()
         {
             // Arrange
             var prefix = string.Empty;
 
             // Act
             Action capture = () =>
             {
                 var sut = new NewRelicMetric(prefix);
             };
 
             // Assert
             Assert.Throws<ArgumentException>(capture);
         }
        
        [Fact]
        public void Constructor_WhenGivenWhitespacePrefix_ThrowsArgumentException()
        {
            // Arrange
            var prefix = "   ";
         
            // Act
            Action capture = () =>
            {
                var sut = new NewRelicMetric(prefix);
            };
         
            // Assert
            Assert.Throws<ArgumentException>(capture);
        }
        
        [Fact]
        public void Constructor_WhenGivenValidPrefix_CreatesInstance()
        {
            // Arrange
            var prefix = "Vestigen";

            // Act
            var sut = new NewRelicMetric(prefix);

            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void Constructor_WhenGivenNullSettingsInstance_ThrowsArgumentNullException()
        {
            // Arrange
            var service = default(INewRelicMetricSettings);

            // Act
            Action capture = () =>
            {
                var sut = new NewRelicMetric(service);
            };

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }
        
        [Fact]
        public void Constructor_WhenGivenValidSettingsInstance_CreatesInstance()
        {
            // Arrange
            var service = new Mock<INewRelicMetricSettings>();
            service.SetupGet(x => x.Prefix).Returns("TestPrefix");

            // Act
            var sut = new NewRelicMetric(service.Object);

            // Assert
            Assert.NotNull(sut);
            service.VerifyGet(x => x.Prefix, Times.Exactly(2));
        }       

        [Fact]
        public void Constructor_WhenGivenStaticMetricSettingsWithMetricNameOnly_CreatesInstance()
        {
            // Arrange
            var settings = new NewRelicMetricSettings { Prefix = "TestPrefix" };

            // Act
            var sut = new NewRelicMetric(settings);

            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void Push_WhenGivenValidTimerDataAnd0ChainOfScopes_ReportsTimerData()
        {
            // Arrange
            const string statistic = "PushDataTest";
            
            var listener = new NewRelicTraceListener();
            Trace.Listeners.Add(listener);
            
            var sut = new NewRelicMetric("TestPrefix");

            // Act
            sut.Push(MetricType.Timer, "PushDataTest", 1, 1, new string[]{} );

            // Assert
            //Assert.Equal("NewRelic.RecordResponseTimeMetric(name,millis)", listener.Strings[2]);
            Trace.Listeners.Remove(listener);
        }
        
        [Fact]
        public void Push_WhenGivenValidTimerDataAnd1ChainOfScopes_ReportsTimerData()
        {
            // Arrange
            const string statistic = "Scope1.PushDataTest";
            
            var listener = new NewRelicTraceListener();
            Trace.Listeners.Add(listener);
            
            var sut = new NewRelicMetric("TestPrefix");
            var scope = sut.BeginScope("Scope1");

            // Act
            sut.Push(MetricType.Timer, "PushDataTest", 1, 1, new string[]{} );

            // Assert
            scope.Dispose();
            //Assert.Equal("NewRelic.RecordResponseTimeMetric(name,millis)", listener.Strings[2]);
            Trace.Listeners.Remove(listener);
        }
    }
}
