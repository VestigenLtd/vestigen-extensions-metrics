using System;
using System.Collections.Generic;
using Moq;
using Vestigen.Extensions.Metrics.Abstractions;
using Xunit;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable ObjectCreationAsStatement

namespace Vestigen.Extensions.Metrics.UnitTests
{
    public class MetricTest
    {
        [Fact]
        public void Constructor_WhenGivenNullFactoryAndNullName_ThrowsArgumentNullException()
        {
            // Arrange
            
            // Act
            Action capture = () => new Metric(null, null);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }

        [Fact]
        public void Constructor_WhenGivenValidFactoryAndNullName_ThrowsArgumentNullException()
        {
            // Arrange
            var factory = new Mock<IMetricFactory>();

            // Act
            Action capture = () => new Metric(factory.Object, null);

            // Assert
            Assert.Throws<ArgumentNullException>(capture);
        }

        [Fact]
        public void Constructor_WhenGivenValidFactoryAndEmptyName_ThrowsArgumentException()
        {
            // Arrange
            var factory = new Mock<IMetricFactory>();
            var category = "";

            // Act
            Action capture = () => new Metric(factory.Object, category);

            // Assert
            Assert.Throws<ArgumentException>(capture);
        }

        [Fact]
        public void Constructor_WhenGivenValidFactoryAndValidName_Succeeds()
        {
            // Arrange
            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider>());

            const string category = "TestCategory";

            // Act
            new Metric(factory.Object, category);

            // Assert
        }

        [Fact]
        public void BeginScope_WhenFactoryContainsNoProviders_ReturnsScope()
        {
            // Arrange
            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider>());

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            var capture = metric.BeginScope("Scope1");

            // Assert
            Assert.NotNull(capture);
        }

        [Fact]
        public void BeginScope_WhenFactoryContainsSingleProviders_ReturnsScope()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.BeginScope(It.IsAny<string>())).Returns(new Mock<IDisposable>().Object);

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            var capture = metric.BeginScope("Scope1");

            // Assert
            Assert.NotNull(capture);
        }

        [Fact]
        public void BeginScope_WhenFactoryContainsMultipleProviders_ReturnsScope()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.BeginScope(It.IsAny<string>())).Returns(new Mock<IDisposable>().Object);

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object, provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            var capture = metric.BeginScope("Scope1");

            // Assert
            Assert.NotNull(capture);
        }

        [Fact]
        public void BeginScope_WhenFactoryProviderMetricThrowsOnBeginScope_ThrowsAggregateException()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.BeginScope(It.IsAny<string>())).Throws<InvalidOperationException>();

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object, provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            Action capture = () => metric.BeginScope("Scope1");

            // Assert
            Assert.Throws<AggregateException>(capture);
        }

        [Fact]
        public void Push_WhenFactoryContainsNoProviders_Succeeds()
        {
            // Arrange
            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider>());

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Push(MetricType.Counter, "TestCounter", 1, 1.0);

            // Assert
        }

        [Fact]
        public void Push_WhenFactoryContainsSingleProviders_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Push(MetricType.Counter, "TestCounter", 1, 1.0);

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null), Times.Once);
        }

        [Fact]
        public void Push_WhenFactoryContainsMultipleProviders_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object, provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Push(MetricType.Counter, "TestCounter", 1, 1.0);

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null), Times.Exactly(2));
        }

        [Fact]
        public void Push_WhenFactoryProviderMetricThrowsOnPush_ThrowsAggregateException()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null)).Throws<InvalidOperationException>();

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object, provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            Action capture = () => metric.Push(MetricType.Counter, "TestCounter", 1, 1.0);

            // Assert
            Assert.Throws<AggregateException>(capture);
        }

        [Fact]
        public void Counter_WhenGivenValidValuesAndValidProvider_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Counter("TestCounter", 1);

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null), Times.Once);
        }

        [Fact]
        public void Increment_WhenGivenValidValuesAndValidProvider_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Increment("TestCounter");

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null), Times.Once);
        }

        [Fact]
        public void Decrement_WhenGivenValidValuesAndValidProvider_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Decrement("TestCounter");

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Counter, "TestCounter", 1, 1.0, null), Times.Once);
        }

        [Fact]
        public void Gauge_WhenGivenValidValuesAndValidProvider_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Gauge, "TestCounter", 1, 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Gauge("TestCounter", 1);

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Gauge, "TestCounter", 1, 1.0, null), Times.Once);
        }

        [Fact]
        public void Histogram_WhenGivenValidValuesAndValidProvider_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Histogram, "TestCounter", 1, 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Histogram("TestCounter", 1);

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Histogram, "TestCounter", 1, 1.0, null), Times.Once);
        }

        [Fact]
        public void Set_WhenGivenValidValuesAndValidProvider_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Set, "TestCounter", 1, 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Set("TestCounter", 1);

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Set, "TestCounter", 1, 1.0, null), Times.Once);
        }

        [Fact]
        public void Timer_WhenGivenValidValuesAndValidProvider_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Timer, "TestCounter", 1, 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Timer("TestCounter", 1);

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Timer, "TestCounter", 1, 1.0, null), Times.Once);
        }

        [Fact]
        public void StartTimer_WhenGivenValidValuesAndValidProvider_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Timer, "TestCounter", It.IsAny<long>(), 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            var capture = metric.StartTimer("TestCounter");
            capture.Dispose();

            // Assert
            Assert.NotNull(capture);
            providerMetric.Verify(m => m.Push(MetricType.Timer, "TestCounter", It.IsAny<long>(), 1.0, null), Times.Once);
        }

        [Fact]
        public void Time_WhenGivenValidActionAndValidProvider_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Timer, "TestCounter", It.IsAny<long>(), 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Time(() => {}, "TestCounter");

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Timer, "TestCounter", It.IsAny<long>(), 1.0, null), Times.Once);
        }

        [Fact]
        public void Time_WhenGivenValidFuncAndValidProvider_Succeeds()
        {
            // Arrange
            var providerMetric = new Mock<IMetric>();
            providerMetric.Setup(m => m.Push(MetricType.Timer, "TestCounter", It.IsAny<long>(), 1.0, null));

            var provider = new Mock<IMetricProvider>();
            provider.Setup(m => m.CreateMetric(It.IsAny<string>())).Returns(providerMetric.Object);

            var factory = new Mock<IMetricFactory>();
            factory.Setup(m => m.GetProviders()).Returns(new List<IMetricProvider> { provider.Object });

            var metric = new Metric(factory.Object, "TestMetric");

            // Act
            metric.Time(() => false, "TestCounter");

            // Assert
            providerMetric.Verify(m => m.Push(MetricType.Timer, "TestCounter", It.IsAny<long>(), 1.0, null), Times.Once);
        }
    }
}
