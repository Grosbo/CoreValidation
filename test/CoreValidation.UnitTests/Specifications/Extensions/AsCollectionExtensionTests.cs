using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Extensions
{
    public class AsCollectionExtensionTests
    {
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void AssertCollectionRuleAdded(IReadOnlyCollection<ICommand> commands, object itemSpecification)
        {
            Assert.Single(commands);
            Assert.IsType<AsCollectionRule<object, int>>(commands.Single());

            var command = (AsCollectionRule<object, int>)commands.Single();

            Assert.Equal("AsCollection", command.Name);
            Assert.Null(command.RuleSingleError);
            Assert.Same(itemSpecification, command.ItemSpecification);
            Assert.NotNull(command.ItemSpecification);
        }

        [Fact]
        public void Should_Add_AsCollectionRule_When_AsCollection_And_Array()
        {
            var builder = new MemberSpecificationBuilder<object, int[]>();

            var itemSpecification = new MemberSpecification<object, int>(x => x);

            builder.AsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }


        [Fact]
        public void Should_Add_AsCollectionRule_When_AsCollection_And_Collection()
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            var itemSpecification = new MemberSpecification<object, int>(x => x);

            builder.AsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }

        [Fact]
        public void Should_Add_AsCollectionRule_When_AsCollection_And_ICollection()
        {
            var builder = new MemberSpecificationBuilder<object, ICollection<int>>();

            var itemSpecification = new MemberSpecification<object, int>(x => x);

            builder.AsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }

        [Fact]
        public void Should_Add_AsCollectionRule_When_AsCollection_And_IEnumerable()
        {
            var builder = new MemberSpecificationBuilder<object, IEnumerable<int>>();

            var itemSpecification = new MemberSpecification<object, int>(x => x);

            builder.AsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }

        [Fact]
        public void Should_Add_AsCollectionRule_When_AsCollection_And_IList()
        {
            var builder = new MemberSpecificationBuilder<object, IList<int>>();

            var itemSpecification = new MemberSpecification<object, int>(x => x);

            builder.AsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }


        [Fact]
        public void Should_Add_AsCollectionRule_When_AsCollection_And_IReadOnlyCollection()
        {
            var builder = new MemberSpecificationBuilder<object, IReadOnlyCollection<int>>();

            var itemSpecification = new MemberSpecification<object, int>(x => x);

            builder.AsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }


        [Fact]
        public void Should_Add_AsCollectionRule_When_AsCollection_And_List()
        {
            var builder = new MemberSpecificationBuilder<object, List<int>>();

            var itemSpecification = new MemberSpecification<object, int>(x => x);

            builder.AsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }


        [Fact]
        public void Should_Add_AsCollectionRule_When_AsCollection_And_ReadOnlyCollection()
        {
            var builder = new MemberSpecificationBuilder<object, ReadOnlyCollection<int>>();

            var itemSpecification = new MemberSpecification<object, int>(x => x);

            builder.AsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }
    }
}