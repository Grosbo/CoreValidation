using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Extensions
{
    public class AsModelExtensionTests
    {
        private class MemberClass
        {
        }

        [Fact]
        public void Should_Add_AsModelRule_When_AsModel()
        {
            var builder = new MemberSpecificationBuilder<object, MemberClass>();

            var memberSpecification = new Specification<MemberClass>(x => x);

            builder.AsModel(memberSpecification);

            Assert.Single(builder.Commands);
            Assert.IsType<AsModelRule<MemberClass>>(builder.Commands.Single());

            var command = (AsModelRule<MemberClass>)builder.Commands.Single();

            Assert.Equal("AsModel", command.Name);
            Assert.Null(command.RuleSingleError);
            Assert.Same(memberSpecification, command.Specification);
            Assert.NotNull(command.Specification);
        }

        [Fact]
        public void Should_Add_AsModelRule_When_AsModel_When_NullSpecification()
        {
            var builder = new MemberSpecificationBuilder<object, MemberClass>();

            builder.AsModel();

            Assert.Single(builder.Commands);
            Assert.IsType<AsModelRule<MemberClass>>(builder.Commands.Single());

            var command = (AsModelRule<MemberClass>)builder.Commands.Single();

            Assert.Equal("AsModel", command.Name);
            Assert.Null(command.RuleSingleError);
            Assert.Null(command.Specification);
        }
    }
}