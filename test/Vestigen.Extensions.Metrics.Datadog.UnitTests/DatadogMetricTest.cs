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
        public void Constructor_WhenGivenValidName_CreatesInstance()
        {
            // Arrange
            var name = string.Empty;

            // Act
            var sut = new DatadogMetric(name);

            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void Constructor_WhenGivenNullName_ThrowsArgumentNullException()
        {
            // Arrange
            var name = default(string);

            // Act
            Action capture = () =>
            {
                var sut = new DatadogMetric(name);
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
        public void Push_WhenGivenValidTimerData_ReportsTimerData()
        {
            // Arrange
            var service = new Mock<IDogStatsd>();
            service.Setup(x => x.Timer(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<double>(), It.IsAny<string[]>())).Verifiable();
            service.CallBase = true;
            var sut = new DatadogMetric(service.Object);

            // Act
            sut.Push<int>(MetricType.Timer, "PushDataTest", 1, 1, new string[]{} );

            // Assert
            service.Verify(x => x.Timer(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<double>(), It.IsAny<string[]>()), Times.Once);
        }
    }
}
