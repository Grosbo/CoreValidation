using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Extensions
{
    public class SetOptionalExtensionTests
    {
        private class MemberClass
        {
        }

        [Fact]
        public void Should_Add_SetOptional_When_SetOptional_NullableType()
        {
            var builderInterface = new MemberSpecificationBuilder<object, int?>() as
                IMemberSpecificationBuilder<object, int?>;

            builderInterface.SetOptional();

            var builder = (MemberSpecificationBuilder<object, int?>)builderInterface;

            Assert.Single(builder.Commands);
            Assert.IsType<SetOptionalCommand>(builder.Commands.Single());
        }

        [Fact]
        public void Should_Add_SetOptional_When_SetOptional_ReferenceType()
        {
            var builderInterface = new MemberSpecificationBuilder<object, MemberClass>() as
                IMemberSpecificationBuilder<object, MemberClass>;

            builderInterface.SetOptional();

            var builder = (MemberSpecificationBuilder<object, MemberClass>)builderInterface;

            Assert.Single(builder.Commands);
            Assert.IsType<SetOptionalCommand>(builder.Commands.Single());
        }
    }
}