using System;
using Moq;
using Vestigen.Extensions.Metrics.Abstractions;
using Xunit;

namespace Vestigen.Extensions.Metrics.UnitTests
{
    public class MetricFactoryTest
    {
        [Fact]
        public void AddProvider_WhenInstanceIsDisposed_ThrowsObjectDisposedException()
        {
            // Arrange
            var disposableProvider = new Mock<IMetricProvider>();
            disposableProvider.Setup(p => p.Dispose());

            var factory = new MetricFactory();

            // Act
            factory.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => factory.AddProvider(disposableProvider.Object));
        }

        [Fact]
        public void AddProvider_WhenGivenInvalidProvider_ThrowsArgumentNullException()
        {
            // Arrange
            var factory = new MetricFactory();

            // Act
            Action capture = () => factory.AddProvider(null);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }

        [Fact]
        public void AddProvider_WhenGivenValidProviderAndNotDisposed_Returns()
        {
            // Arrange
            var factory = new MetricFactory();
            factory.CreateMetric("TestMetric");

            var provider = new Mock<IMetricProvider>();

            // Act
            factory.AddProvider(provider.Object);

            // Assert
            Assert.Single(factory.GetProviders());
        }

        [Fact]
        public void AddProvider_WhenGivenValidProviderAndIsDisposed_ThrowsObjectDisposedException()
        {
            // Arrange
            var factory = new MetricFactory();
            factory.Dispose();
            var provider = new Mock<IMetricProvider>();

            // Act
            Action capture = () => factory.AddProvider(provider.Object);

            // Assert
            Assert.Throws<ObjectDisposedException>(capture);
        }

        [Fact]
        public void CreateMetric_WhenGivenNullValue_ThrowsArgumentNullException()
        {
            // Arrange
            var factory = new MetricFactory();

            // Act
            Action capture = () => factory.CreateMetric(null);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }

        [Fact]
        public void CreateMetric_WhenGivenEmptyValue_ThrowsArgumentException()
        {
            // Arrange
            var factory = new MetricFactory();

            // Act
            Action capture = () => factory.CreateMetric(string.Empty);

            // Assert
            Assert.Throws<ArgumentException>(capture);
        }

        [Fact]
        public void CreateMetric_WhenInstanceIsDisposed_ThrowsObjectDisposedException()
        {
            // Arrange
            var factory = new MetricFactory();

            // Act
            factory.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => factory.CreateMetric("disposedMatric"));
        }

        [Fact]
        public void GetProviders_WhenCalled_ReturnsListOfProviders()
        {
            // Arrange
            var factory = new MetricFactory();

            // Act
            var capture = factory.GetProviders();

            // Assert
            Assert.NotNull(capture);
        }

        [Fact]
        public void Dispose_WhenInstanceIsDisposed_DoesNothing()
        {
            // Arrange
            var factory = new MetricFactory();

            // Act
            factory.Dispose();

            // Assert
            Assert.True(factory.IsDisposed);
            factory.Dispose();
        }

        [Fact]
        public void Dispose_WhenInstanceIsDisposed_EnsuresProvidersAreDisposed()
        {
            // Arrange
            var disposableProvider1 = new Mock<IMetricProvider>();
            disposableProvider1
                .As<IDisposable>()
                .Setup(p => p.Dispose());

            var disposableProvider2 = new Mock<IMetricProvider>();
            disposableProvider2
                .As<IDisposable>()
                .Setup(p => p.Dispose());

            var factory = new MetricFactory();
            factory.AddProvider(disposableProvider1.Object);
            factory.AddProvider(disposableProvider2.Object);

            // Act
            factory.Dispose();

            // Assert
            disposableProvider1.Verify(provider => provider.Dispose(), Times.Once);
            disposableProvider2.Verify(provider => provider.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_WhenInstanceIsDisposed_EnsuresProviderThrownExceptionsAreSwallowed()
        {
            // Arrange       
            var throwingProvider = new Mock<IMetricProvider>();
            throwingProvider
                .Setup(p => p.Dispose())
                .Throws<Exception>();

            var factory = new MetricFactory();
            factory.AddProvider(throwingProvider.Object);

            // Act
            factory.Dispose();

            // Assert
            throwingProvider.Verify(provider => provider.Dispose(), Times.Once());
        }
    }
}
