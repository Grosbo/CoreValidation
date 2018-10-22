using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Extensions
{
    public class AsModelsCollectionExtensionTests
    {
        private void AssertCollectionRuleAdded(IReadOnlyCollection<ICommand> commands, object itemSpecification)
        {
            Assert.Single(commands);
            Assert.IsType<AsCollectionRule<object, MemberClass>>(commands.Single());

            var command = (AsCollectionRule<object, MemberClass>)commands.Single();

            Assert.Equal("AsCollection", command.Name);
            Assert.Null(command.RuleSingleError);
            Assert.NotNull(command.ItemSpecification);

            Assert.False(command.MemberValidator.IsOptional);

            Assert.Single(command.MemberValidator.Rules);

            Assert.IsType<AsModelRule<MemberClass>>(command.MemberValidator.Rules.Single());

            var asModelRule = (AsModelRule<MemberClass>)command.MemberValidator.Rules.Single();

            Assert.Same(itemSpecification, asModelRule.Specification);

            // just for resharper warnings
            // ReSharper disable once UnusedVariable
            var void1 = itemSpecification;
        }

        public static IEnumerable<object[]> AddModelsCollection_Data()
        {
            var specifications = new object[]
            {
                new Specification<MemberClass>(x => x),
                null
            };


            foreach (var specification in specifications)
            {
                yield return new[]
                {
                    specification
                };
            }
        }

        [Theory]
        [MemberData(nameof(AddModelsCollection_Data))]
        public void Should_Add_AsModelsCollectionRule_When_AsModelsCollection_And_Array(Specification<MemberClass> itemSpecification)
        {
            var builder = new MemberSpecificationBuilder<object, MemberClass[]>();

            builder.AsModelsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }

        [Theory]
        [MemberData(nameof(AddModelsCollection_Data))]
        public void Should_Add_AsModelsCollectionRule_When_AsModelsCollection_And_IEnumerable(Specification<MemberClass> itemSpecification)
        {
            var builder = new MemberSpecificationBuilder<object, IEnumerable<MemberClass>>();

            builder.AsModelsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }

        [Theory]
        [MemberData(nameof(AddModelsCollection_Data))]
        public void Should_Add_AsModelsCollectionRule_When_AsModelsCollection_And_ICollection(Specification<MemberClass> itemSpecification)
        {
            var builder = new MemberSpecificationBuilder<object, ICollection<MemberClass>>();

            builder.AsModelsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }

        [Theory]
        [MemberData(nameof(AddModelsCollection_Data))]
        public void Should_Add_AsModelsCollectionRule_When_AsModelsCollection_And_Collection(Specification<MemberClass> itemSpecification)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<MemberClass>>();

            builder.AsModelsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }

        [Theory]
        [MemberData(nameof(AddModelsCollection_Data))]
        public void Should_Add_AsModelsCollectionRule_When_AsModelsCollection_And_IReadOnlyCollection(Specification<MemberClass> itemSpecification)
        {
            var builder = new MemberSpecificationBuilder<object, IReadOnlyCollection<MemberClass>>();

            builder.AsModelsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }


        [Theory]
        [MemberData(nameof(AddModelsCollection_Data))]
        public void Should_Add_AsModelsCollectionRule_When_AsModelsCollection_And_ReadOnlyCollection(Specification<MemberClass> itemSpecification)
        {
            var builder = new MemberSpecificationBuilder<object, ReadOnlyCollection<MemberClass>>();

            builder.AsModelsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }


        [Theory]
        [MemberData(nameof(AddModelsCollection_Data))]
        public void Should_Add_AsModelsCollectionRule_When_AsModelsCollection_And_IList(Specification<MemberClass> itemSpecification)
        {
            var builder = new MemberSpecificationBuilder<object, IList<MemberClass>>();

            builder.AsModelsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }

        [Theory]
        [MemberData(nameof(AddModelsCollection_Data))]
        public void Should_Add_AsModelsCollectionRule_When_AsModelsCollection_And_List(Specification<MemberClass> itemSpecification)
        {
            var builder = new MemberSpecificationBuilder<object, List<MemberClass>>();

            builder.AsModelsCollection(itemSpecification);

            AssertCollectionRuleAdded(builder.Commands, itemSpecification);
        }

        public class MemberClass
        {
        }
    }
}