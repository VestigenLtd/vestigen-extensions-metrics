using System;
using Vestigen.Extensions.Metrics.Datadog;
using Xunit;
using Xunit.Abstractions;

namespace Vestigen.Extensions.Metrics.Debug.UnitTests
{
    public class DebugMetricTest
    {
        private readonly ITestOutputHelper _output;

        public DebugMetricTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void Constructor_WhenGivenValidName_CreatesInstance()
        {
            // Arrange
            var name = string.Empty;

            // Act
            var sut = new DebugMetric(name);

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
                var sut = new DebugMetric(name);
            };

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }
        
        [Fact]
        public void Constructor_WhenGivenValidServiceInstance_CreatesInstance()
        {
            // Arrange
            var settings = new DebugMetricSettings();

            // Act
            var sut = new DebugMetric(settings);

            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void Constructor_WhenGivenNullServiceInstancee_ThrowsArgumentNullException()
        {
            // Arrange
            var settings = default(IDebugMetricSettings);

            // Act
            Action capture = () =>
            {
                var sut = new DebugMetric(settings);
            };

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }

        [Fact]
        public void Constructor_WhenGivenStaticMetricSettingsWithMetricNameOnly_CreatesInstance()
        {
            // Arrange
            var settings = new DebugMetricSettings { Prefix = "metricprefix" };

            // Act
            var sut = new DebugMetric(settings);

            // Assert
            Assert.NotNull(sut);
        }
    }
}
