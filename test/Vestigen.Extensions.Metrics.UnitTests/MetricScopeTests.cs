using System;
using System.Collections.Generic;
using Moq;
using Vestigen.Extensions.Metrics.Abstractions;
using Xunit;

namespace Vestigen.Extensions.Metrics.UnitTests
{
    public class MetricScopeTests
    {

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

    }
}
