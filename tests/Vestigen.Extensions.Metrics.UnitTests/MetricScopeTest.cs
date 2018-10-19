using Xunit;
using Xunit.Abstractions;

namespace Vestigen.Extensions.Metrics.UnitTests
{
    public class MetricScopeTest
    {
        private ITestOutputHelper _output;

        public MetricScopeTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void CompleteName_WhenGiven0ChainOfScopes_ReturnsExpectedString()
        {
            // Arrange
            var sut = MetricScope.Current;

            // Act
            var capture = sut.CompleteName;

            // Assert
            _output.WriteLine(capture);
            Assert.Equal(string.Empty, capture);
        }
        
        [Fact]
        public void CompleteName_WhenGiven1ChainOfScopes_ReturnsExpectedString()
        {
            // Arrange
            var scope1 = MetricScope.Push("Scope1");
            var sut = MetricScope.Current;

            // Act
            var capture = sut.CompleteName;

            // Assert
            _output.WriteLine(capture);
            Assert.Equal("Scope1", capture);
            scope1.Dispose();
        }
        
        [Fact]
        public void CompleteName_WhenGiven2ChainOfScopes_ReturnsExpectedString()
        {
            // Arrange
            var scope1 = MetricScope.Push("Scope1");
            var scope2 = MetricScope.Push("Scope2");
            var sut = MetricScope.Current;

            // Act
            var capture = sut.CompleteName;

            // Assert
            _output.WriteLine(capture);
            Assert.Equal("Scope1.Scope2", capture);
            scope1.Dispose();
            scope2.Dispose();
        }
    }
}