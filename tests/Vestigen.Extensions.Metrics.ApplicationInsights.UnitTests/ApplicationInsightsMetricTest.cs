using System;
using Microsoft.ApplicationInsights;
using Vestigen.Extensions.Metrics.ApplicationInsights;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace Vestigen.Extensions.Metrics.CloudWatch.UnitTests
{
    public class ApplicationInsightsMetricTest : IClassFixture<ApplicationInsightsMetricTestFixture>, IDisposable
    {
        private readonly ApplicationInsightsMetricTestFixture _fixture;

        public ApplicationInsightsMetricTest(ApplicationInsightsMetricTestFixture fixture)
        {
            // See remarks on CloudWatchMetricTestFixture for why this is here
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        [Fact]
        public void Constructor_WhenGivenNullNamespaceOnly_ThrowsArgumentNullException()
        {
            // Arrange
            const string ns = default(string);

            // Act
            Action capture = () => new ApplicationInsightsMetric(ns);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }
        
        [Fact]
        public void Constructor_WhenGivenEmptyNamespaceOnly_ThrowsArgumentException()
        {
            // Arrange
            var ns = string.Empty;

            // Act
            Action capture = () => new ApplicationInsightsMetric(ns);

            // Assert
            Assert.Throws<ArgumentException>(capture);
        }
        
        [Fact]
        public void Constructor_WhenGivenValidNamespaceOnly_CreatesInstance()
        {
            // Arrange
            const string ns = "Vestigen/";

            // Act
            var sut = new ApplicationInsightsMetric(ns);

            // Assert
            Assert.NotNull(sut);
        }
        
        [Fact]
        public void Constructor_WhenGivenNullSettingsOnly_ThrowsArgumentNullException() {}
        
        [Fact]
        public void Constructor_WhenGivenValidSettingsOnly_CreatesInstance() {}

        [Fact]
        public void Constructor_WhenGivenValidNamespaceAndNullConfig_ThrowsArgumentNullException()
        {
            // Arrange
            const string ns = "Vestigen/";

            // Act
            Action capture = () => new ApplicationInsightsMetric(ns, null);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }

        [Fact]
        public void Constructor_WhenGivenValidNamespaceAndValidConfig_CreatesInstance()
        {
            // Arrange
            const string ns = "Vestigen/";
            var config = new TelemetryClient();

            // Act
            var sut = new ApplicationInsightsMetric(ns, config);

            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void Constructor_WhenGivenValidNamespaceAndNullClient_ThrowsArgumentNullException()
        {
            // Arrange
            const string ns = "Vestigen/";

            // Act
            Action capture = () => new ApplicationInsightsMetric(ns, null);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }

        [Fact]
        public void Constructor_WhenGivenValidNamespaceAndValidClient_CreatesInstance()
        {
            // Arrange
            const string ns = "Vestigen/";
            var config = new TelemetryClient();

            // Act
            var sut = new ApplicationInsightsMetric(ns, config);

            // Assert
            Assert.NotNull(sut);
        }
    }
}