using System;
using Xunit;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ExpressionIsAlwaysNull

namespace Vestigen.Extensions.Metrics.Debug.UnitTests
{
    public class DebugMetricTest
    {      
        [Fact]
        public void Constructor_WhenGivenValidName_CreatesInstance()
        {
            // Arrange
            const string name = "DebugMetric";

            // Act
            var sut = new DebugMetric(name);

            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void Constructor_WhenGivenNullName_ThrowsArgumentNullException()
        {
            // Arrange
            const string name = default(string);

            // Act
            Action capture = () => new DebugMetric(name);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }
        
        [Fact]
        public void Constructor_WhenGivenValidServiceInstance_CreatesInstance()
        {
            // Arrange
            var settings = new DebugMetricSettings
            {
                Prefix = "DebugMetric"
            };

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
            Action capture = () => new DebugMetric(settings);

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
