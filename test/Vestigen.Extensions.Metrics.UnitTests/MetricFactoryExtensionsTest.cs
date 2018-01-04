using Moq;
using Vestigen.Extensions.Metrics.Abstractions;
using Vestigen.Extensions.Metrics.UnitTests.Abstractions;
using Xunit;

// ReSharper disable PossibleNullReferenceException

namespace Vestigen.Extensions.Metrics.UnitTests
{
    public class MetricFactoryExtensionsTest
    {
        [Fact]
        public void CreateMetricOfT_WhenGivenNonGenericTypeAsTypeParameter_CreatesMetricWithNonGenericTypeName()
        {
            // Arrange
            var expected = typeof(TestType).FullName;

            var factory = new Mock<IMetricFactory>();
            factory
                .Setup(f => f.CreateMetric(It.IsAny<string>()))
                .Returns(new Mock<IMetric>().Object);

            // Act
            factory.Object.CreateMetric<TestType>();

            // Assert
            factory.Verify(f => f.CreateMetric(expected));
        }

        [Fact]
        public void CreateMetricOfT_SingleGeneric_CreatesMetricWithNonGenericTypeName()
        {
            // Arrange
            const string className = "Holroyd.Extensions.Metrics.Test.GenericClass<Holroyd.Extensions.Metrics.Test.TestType>";
            var factory = new Mock<IMetricFactory>();
            factory
                .Setup(f => f.CreateMetric(It.Is<string>(x => x.Equals(className))))
                .Returns(new Mock<IMetric>().Object);

            // Act
            var metric = factory.Object.CreateMetric<GenericClass<TestType>>();

            // Assert
            Assert.NotNull(metric);
        }

        [Fact]
        public void CreateMetricOfT_WhenGivenMultipleTypeArgumentGenericTypeByTypeParameter_CreatesMetricWithNonGenericTypeName()
        {
            // Arrange
            const string className = "Holroyd.Extensions.Metrics.Test.GenericClass<Holroyd.Extensions.Metrics.Test.TestType, Holroyd.Extensions.Metrics.Test.SecondTestType>";
            var factory = new Mock<IMetricFactory>();
            factory
                .Setup(f => f.CreateMetric(It.Is<string>(x => x.Equals(className))))
                .Returns(new Mock<IMetric>().Object);

            var metric = factory.Object.CreateMetric<GenericClass<TestType, SecondTestType>>();

            // Assert
            Assert.NotNull(metric);
        }

        [Fact]
        public void CreateMetric_WhenGivenMultipleTypeArgumentGenericTypeByReferenceParameter_CreatesMetricWithNonGenericTypeName()
        {
            // Arrange
            const string className = "Holroyd.Extensions.Metrics.Test.GenericClass";
            var factory = new Mock<IMetricFactory>();
            factory
                .Setup(f => f.CreateMetric(It.Is<string>(x => x.Equals(className))))
                .Returns(new Mock<IMetric>().Object);

            var metric = factory.Object.CreateMetric(typeof(GenericClass<TestType, SecondTestType>));

            // Assert
            Assert.NotNull(metric);
        }

        [Fact]
        public void CreateMetricOfT_WhenGivenSingleTypeArgumentOfGenericTypeByTypeParameter_CreatesMetricWithNonGenericTypeName()
        {
            // Arrange
            var fullName = typeof(GenericClass<string>).GetGenericTypeDefinition().FullName;
            var fullNameWithoutBacktick = fullName.Substring(0, fullName.IndexOf('`'));
            var testSink = new TestSink();
            var factory = new TestMetricFactory(testSink, true);

            // Act
            var metric = factory.CreateMetric<GenericClass<string>>();
            metric.Push(MetricType.Counter, "TestCounter", 1, 1.0);

            // Assert
            Assert.Single(testSink.ReportingContexts);
            Assert.Equal(fullNameWithoutBacktick, testSink.ReportingContexts[0].MetricName);
        }
        
        [Fact]
        public void CreateMetric_WhenGivenSingleTypeArgumentOfGenericTypeByReferenceParameter_CreatesMetricWithNonGenericTypeName()
        {
            // Arrange
            const string className = "Holroyd.Extensions.Metrics.Test.GenericClass";
            var factory = new Mock<IMetricFactory>();
            factory
                .Setup(f => f.CreateMetric(It.Is<string>(x => x.Equals(className))))
                .Returns(new Mock<IMetric>().Object);

            var metric = factory.Object.CreateMetric(typeof(GenericClass<TestType>));

            // Assert
            Assert.NotNull(metric);
        }

        [Fact]
        public void CreateMetric_WhenGivenNestedGenericType_CreatesMectrixWithoutGenericTypeArgumentsInformation()
        {
            // Arrange
            var fullName = typeof(GenericClass<GenericClass<string>>).GetGenericTypeDefinition().FullName;
            var fullNameWithoutBacktick = fullName.Substring(0, fullName.IndexOf('`'));
            var testSink = new TestSink();
            var factory = new TestMetricFactory(testSink, true);

            // Act
            var metric = factory.CreateMetric<GenericClass<GenericClass<string>>>();
            metric.Push(MetricType.Counter, "TestCounter", 1, 1.0);

            // Assert
            Assert.Single(testSink.ReportingContexts);
            Assert.Equal(fullNameWithoutBacktick, testSink.ReportingContexts[0].MetricName);
        }

        [Fact]
        public void CreateMetricOfT_WhenGivenMultipleTypeArgumentOfGenericType_CreatesMetricWithNonGenericTypeName()
        {
            // Arrange
            var fullName = typeof(GenericClass<string, string>).GetGenericTypeDefinition().FullName;
            var fullNameWithoutBacktick = fullName.Substring(0, fullName.IndexOf('`'));
            var testSink = new TestSink();
            var factory = new TestMetricFactory(testSink, true);

            // Act
            var metric = factory.CreateMetric<GenericClass<string, string>>();
            metric.Push(MetricType.Counter, "TestCounter", 1, 1.0);

            // Assert
            Assert.Single(testSink.ReportingContexts);
            Assert.Equal(fullNameWithoutBacktick, testSink.ReportingContexts[0].MetricName);
        }     

        [Fact]
        public void CreateMetric_WhenGivenTypeForName_CreatesMetricWithNonGenericTypeName()
        {
            // Arrange
            var expected = typeof(TestType).FullName;

            var factory = new Mock<IMetricFactory>();
            factory
                .Setup(f => f.CreateMetric(It.IsAny<string>()))
                .Returns(new Mock<IMetric>().Object);

            // Act
            factory.Object.CreateMetric(typeof(TestType));

            // Assert
            factory.Verify(f => f.CreateMetric(expected));
        } 
    }

    internal class TestType
    {
        // intentionally holds nothing
    }

    internal class SecondTestType
    {
        // intentionally holds nothing
    }

    internal class GenericClass<TX, TY>
        where TX : class
        where TY : class
    {
        // intentionally holds nothing
    }

    internal class GenericClass<TX>
        where TX : class
    {
        // intentionally holds nothing
    }
}
