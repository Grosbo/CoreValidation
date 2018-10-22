using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Extensions
{
    public class SetRequiredExtensionTests
    {
        private class MemberClass
        {
        }

        [Fact]
        public void Should_Add_SetRequired_When_SetRequired_NullableType()
        {
            var builderInterface = new MemberSpecificationBuilder<object, int?>() as
                IMemberSpecificationBuilder<object, int?>;

            builderInterface.SetRequired("error");

            var builder = (MemberSpecificationBuilder<object, int?>)builderInterface;

            Assert.Single(builder.Commands);
            Assert.IsType<SetRequiredCommand>(builder.Commands.Single());

            var setRequiredCommand = (SetRequiredCommand)builder.Commands.Single();

            Assert.Equal("error", setRequiredCommand.Message);
        }

        [Fact]
        public void Should_Add_SetRequired_When_SetRequired_ReferenceType()
        {
            var builderInterface = new MemberSpecificationBuilder<object, MemberClass>() as
                IMemberSpecificationBuilder<object, MemberClass>;

            builderInterface.SetRequired("error");

            var builder = (MemberSpecificationBuilder<object, MemberClass>)builderInterface;

            Assert.Single(builder.Commands);
            Assert.IsType<SetRequiredCommand>(builder.Commands.Single());

            var setRequiredCommand = (SetRequiredCommand)builder.Commands.Single();

            Assert.Equal("error", setRequiredCommand.Message);
        }
    }
}