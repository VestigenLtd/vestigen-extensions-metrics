using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

// ReSharper disable ConvertToLocalFunction

namespace Vestigen.Extensions.Metrics.UnitTests
{
    public class MetricServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddMetrics_WhenGivenValidServiceCollection_Succeeds()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var capture = services.AddMetrics();

            // Assert
            Assert.NotNull(capture);
            Assert.Equal(7, capture.Count);
        }

        [Fact]
        public void AddMetrics_WhenGivenNullServiceCollection_ThrowsArgumentNullException()
        {
            // Arrange

            // Act
            Action capture = () => MetricServiceCollectionExtensions.AddMetrics(null);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }
    }
}
