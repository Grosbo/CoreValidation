using CoreValidation.Validators;
using Xunit;

namespace CoreValidation.UnitTests.Validators
{
    public class CommandExceptionTests
    {
        [Fact]
        public void Should_AssignPropertiesFromConstructor()
        {
            var exception = new CommandException("some message", 123, "MemberName");

            Assert.Equal("some message", exception.Message);
            Assert.Equal(123, exception.Order);
            Assert.Equal("MemberName", exception.Name);
        }
    }
}