using System;
using Moq;
using StatsdClient;
using Vestigen.Extensions.Metrics.Abstractions;
using Xunit;

namespace Vestigen.Extensions.Metrics.Datadog.UnitTests
{
    public class DatadogMetricTest
    {
        [Fact]
        public void Constructor_WhenGivenNullPrefix_ThrowsArgumentNullException()
        {
            // Arrange
            var prefix = default(string);

            // Act
            Action capture = () =>
            {
                var sut = new DatadogMetric(prefix);
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
                 var sut = new DatadogMetric(prefix);
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
                var sut = new DatadogMetric(prefix);
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
            var sut = new DatadogMetric(prefix);

            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void Constructor_WhenGivenNullServiceInstance_ThrowsArgumentNullException()
        {
            // Arrange
            var service = default(IDogStatsd);

            // Act
            Action capture = () =>
            {
                var sut = new DatadogMetric(service);
            };

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }
        
        [Fact]
        public void Constructor_WhenGivenValidServiceInstance_CreatesInstance()
        {
            // Arrange
            var service = new Mock<IDogStatsd>();

            // Act
            var sut = new DatadogMetric(service.Object);

            // Assert
            Assert.NotNull(sut);
        }       

        [Fact]
        public void Constructor_WhenGivenStaticMetricSettingsWithMetricNameOnly_CreatesInstance()
        {
            // Arrange
            var settings = new DatadogMetricSettings { Prefix = "metricprefix" };

            // Act
            var sut = new DatadogMetric(settings);

            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void Push_WhenGivenValidTimerDataAnd0ChainOfScopes_ReportsTimerData()
        {
            // Arrange
            const string statistic = "PushDataTest";
            var service = new Mock<IDogStatsd>();
            service.Setup(x => x.Timer(statistic, It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string[]>())).Verifiable();
            var sut = new DatadogMetric(service.Object);

            // Act
            sut.Push(MetricType.Timer, "PushDataTest", 1d, 1, new string[]{} );

            // Assert
            service.Verify(x => x.Timer(statistic, It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string[]>()), Times.Once);
        }
        
        [Fact]
        public void Push_WhenGivenValidTimerDataAnd1ChainOfScopes_ReportsTimerData()
        {
            // Arrange
            var scope1 = MetricScope.Push("Scope1");
            const string statistic = "Scope1.PushDataTest";
            
            var service = new Mock<IDogStatsd>();
            service.Setup(x => x.Timer(statistic, It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string[]>())).Verifiable();
            service.CallBase = true;
            
            var sut = new DatadogMetric(service.Object);

            // Act
            sut.Push(MetricType.Timer, "PushDataTest", 1d, 1, new string[]{} );

            // Assert
            service.Verify(x => x.Timer(statistic, It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string[]>()), Times.Once);
            scope1.Dispose();
        }
    }
}
