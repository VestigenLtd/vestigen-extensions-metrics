using System;
using Amazon.CloudWatch;
using Amazon.Runtime;
using Moq;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace Vestigen.Extensions.Metrics.CloudWatch.UnitTests
{
    public class CloudWatchMetricTest : IClassFixture<CloudWatchMetricTestFixture>, IDisposable
    {
        private readonly CloudWatchMetricTestFixture _fixture;

        public CloudWatchMetricTest(CloudWatchMetricTestFixture fixture)
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
            Action capture = () => new CloudWatchMetric(ns);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }
        
        [Fact]
        public void Constructor_WhenGivenEmptyNamespaceOnly_ThrowsArgumentException()
        {
            // Arrange
            var ns = string.Empty;

            // Act
            Action capture = () => new CloudWatchMetric(ns);

            // Assert
            Assert.Throws<ArgumentException>(capture);
        }
        
        [Fact]
        public void Constructor_WhenGivenValidNamespaceOnly_CreatesInstance()
        {
            // Arrange
            const string ns = "Vestigen/";

            // Act
            var sut = new CloudWatchMetric(ns);

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
            Action capture = () => new CloudWatchMetric(ns, config: null);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }

        [Fact]
        public void Constructor_WhenGivenValidNamespaceAndValidConfig_CreatesInstance()
        {
            // Arrange
            const string ns = "Vestigen/";
            var config = new AmazonCloudWatchConfig();

            // Act
            var sut = new CloudWatchMetric(ns, config);

            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void Constructor_WhenGivenValidNamespaceAndNullClient_ThrowsArgumentNullException()
        {
            // Arrange
            const string ns = "Vestigen/";

            // Act
            Action capture = () => new CloudWatchMetric(ns, client: null);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }

        [Fact]
        public void Constructor_WhenGivenValidNamespaceAndValidClient_CreatesInstance()
        {
            // Arrange
            const string ns = "Vestigen/";
            var client = new Mock<IAmazonCloudWatch>(); //AmazonCloudWatchClient();

            // Act
            var sut = new CloudWatchMetric(ns, client.Object);

            // Assert
            Assert.NotNull(sut);
        }
    }
}