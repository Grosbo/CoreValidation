using CoreValidation.Exceptions;
using Xunit;

namespace CoreValidation.UnitTests.Exceptions
{
    public class InvalidCommandExceptionTests
    {
        [Fact]
        public void Should_AssignMaxDepth()
        {
            var ex = new InvalidCommandException("test");

            Assert.Equal("test", ex.Message);
        }
    }
}