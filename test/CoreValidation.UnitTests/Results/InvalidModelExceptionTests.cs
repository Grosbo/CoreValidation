using CoreValidation.Results;
using Xunit;

namespace CoreValidation.UnitTests.Results
{
    public class InvalidModelExceptionTests
    {
        private class SomeClass
        {
        }

        [Fact]
        public void Should_Assign_MessageAndType()
        {
            var exception = new InvalidModelException<SomeClass>("some message");

            Assert.Equal("some message", exception.Message);
            Assert.Equal(typeof(SomeClass), exception.Type);
        }
    }
}