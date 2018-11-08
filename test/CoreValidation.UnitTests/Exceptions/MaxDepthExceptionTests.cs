using CoreValidation.Exceptions;
using Xunit;

namespace CoreValidation.UnitTests.Exceptions
{
    public class MaxDepthExceptionTests
    {
        [Fact]
        public void Should_AssignMaxDepth()
        {
            var ex = new MaxDepthExceededException(10);

            Assert.Equal(10, ex.MaxDepth);
        }
    }
}