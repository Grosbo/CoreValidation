using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Extensions
{
    public class AsNullableExtensionTests
    {
        [Fact]
        public void Should_Add_AsNullableRule_When_AsNullable()
        {
            var builder = new MemberSpecificationBuilder<object, int?>();

            var memberSpecification = new MemberSpecification<object, int>(x => x);

            builder.AsNullable(memberSpecification);

            Assert.Single(builder.Commands);
            Assert.IsType<AsNullableRule<object, int>>(builder.Commands.Single());

            var command = (AsNullableRule<object, int>)builder.Commands.Single();

            Assert.Equal("AsNullable", command.Name);
            Assert.Null(command.RuleSingleError);
            Assert.Same(memberSpecification, command.MemberSpecification);
        }
    }
}