using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Rules;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class MemberSpecificationBuilderTests
    {
        public static IEnumerable<object[]> ValidErrorAndArgsCombinations_Data()
        {
            var args = new IMessageArg[] {new NumberArg("test1", 1), new TextArg("test2", "two")};

            yield return new object[] {"message", args};
            yield return new object[] {"message", null};
            yield return new object[] {null, null};
        }

        public static IEnumerable<object[]> ArgsCombinations_Data()
        {
            var args = new IMessageArg[] {new NumberArg("test1", 1), new TextArg("test2", "two")};

            yield return new object[] {args};
            yield return new object[] {null};
        }

        public class WithSummaryError
        {
            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(MemberSpecificationBuilderTests))]
            public void Should_SummaryError_BeAdded(IMessageArg[] args)
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                builder.WithSummaryError("message", args);

                Assert.Equal("message", builder.SummaryError.Message);
                Assert.Same(args, builder.SummaryError.Arguments);
            }

            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(MemberSpecificationBuilderTests))]
            public void Should_ThrowException_When_SummaryError_Added_With_NullMessage(IMessageArg[] args)
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Throws<ArgumentNullException>(() => { builder.WithSummaryError(null, args); });
            }

            [Fact]
            public void Should_SummaryError_NotBeAdded_When_NoMethod()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Null(builder.SummaryError);
            }

            [Fact]
            public void Should_ThrowException_When_SummaryError_Added_MultipleTimes()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Throws<InvalidOperationException>(() =>
                {
                    builder
                        .WithSummaryError("message1")
                        .WithSummaryError("message2");
                });
            }
        }

        public class WithRequiredError
        {
            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(MemberSpecificationBuilderTests))]
            public void Should_RequiredError_BeAdded(IMessageArg[] args)
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                builder.WithRequiredError("message", args);

                Assert.Equal("message", builder.RequiredError.Message);
                Assert.Same(args, builder.RequiredError.Arguments);
            }

            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(MemberSpecificationBuilderTests))]
            public void Should_ThrowException_When_RequiredError_Added_With_NullMessage(IMessageArg[] args)
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Throws<ArgumentNullException>(() => { builder.WithRequiredError(null, args); });
            }

            [Fact]
            public void Should_RequiredError_NotBeAdded_When_NoMethod()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Null(builder.RequiredError);
            }

            [Fact]
            public void Should_ThrowException_When_RequiredError_Added_MultipleTimes()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Throws<InvalidOperationException>(() =>
                {
                    builder
                        .WithRequiredError("message1")
                        .WithRequiredError("message2");
                });
            }
        }

        public class WithName
        {
            [Theory]
            [InlineData("test")]
            [InlineData("__            *")]
            public void Should_SetName_When_WithName(string name)
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                builder.WithName(name);

                Assert.Equal(name, builder.Name);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("      ")]
            [InlineData("  \n    ")]
            [InlineData("  \r\n    ")]
            public void Should_ThrowException_When_WithName_And_InvalidName(string name)
            {
                var builder = new MemberSpecificationBuilder<object, object>();
                void Rename() => builder.WithName(name);

                if (name == null)
                {
                    Assert.Throws<ArgumentNullException>((Action)Rename);
                }
                else
                {
                    Assert.Throws<ArgumentException>((Action)Rename);
                }
            }

            [Fact]
            public void Should_ThrowException_When_SetName_MoreThanOnce()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                var builderWithName = builder.WithName("test1");

                Assert.Throws<InvalidOperationException>(() => { builderWithName.WithName("test2"); });
            }
        }

        public class AddingRule
        {
            public static IEnumerable<object[]> Should_AddRule_Data()
            {
                yield return new object[] {new ValidRelativeRule<object>(c => true)};
                yield return new object[] {new ValidRule<object>(c => true)};
                yield return new object[] {new ValidModelRule<object>(c => c)};
                yield return new object[] {new ValidModelRule<object>()};
                yield return new object[] {new ValidNullableRule<object, int>(c => c)};
            }

            [Theory]
            [MemberData(nameof(Should_AddRule_Data))]
            public void Should_AddRule(IRule rule)
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                builder.AddRule(rule);

                Assert.Same(rule, builder.Rules.Single());
            }

            [Fact]
            public void Should_AddRule_MultipleRules()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                var rules = new IRule[]
                {
                    new ValidRelativeRule<object>(c => true),
                    new ValidRule<object>(c => true),
                    new ValidModelRule<object>(c => c),
                    new ValidModelRule<object>(),
                    new ValidNullableRule<object, int>(c => c)
                };

                for (var i = 0; i < rules.Length; ++i)
                {
                    builder.AddRule(rules.ElementAt(i));
                }

                Assert.Equal(rules.Length, builder.Rules.Count);

                for (var i = 0; i < rules.Length; ++i)
                {
                    Assert.Same(rules.ElementAt(i), builder.Rules.ElementAt(i));
                }
            }

            [Fact]
            public void Should_ThrowException_When_AddRule_And_NullArgument()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Throws<ArgumentNullException>(() => { builder.AddRule(null); });
            }
        }

        public class Valid
        {
            [Theory]
            [MemberData(nameof(ValidErrorAndArgsCombinations_Data), MemberType = typeof(MemberSpecificationBuilderTests))]
            public void Should_AddValidRule(string message, IMessageArg[] args)
            {
                Predicate<object> isValid = c => true;

                var builder = new MemberSpecificationBuilder<object, object>();

                builder.Valid(isValid, message, args);

                Assert.IsType<ValidRule<object>>(builder.Rules.Single());

                var memberRule = (ValidRule<object>)builder.Rules.Single();

                Assert.Same(isValid, memberRule.IsValid);

                if (message != null)
                {
                    Assert.Equal(message, memberRule.Error.Message);
                    Assert.Same(args, memberRule.Error.Arguments);
                }
                else
                {
                    Assert.Null(memberRule.Error);
                }
            }

            [Fact]
            public void Should_ThrowException_When_Valid_And_ArgsWithoutMessage()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Throws<ArgumentException>(() => { builder.Valid(c => true, null, new IMessageArg[] {new NumberArg("test", 1)}); });
            }

            [Fact]
            public void Should_ThrowException_When_Valid_And_NullPredicate()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Throws<ArgumentNullException>(() => { builder.Valid(null, "test"); });
            }
        }

        public class ValidRelative
        {
            [Theory]
            [MemberData(nameof(ValidErrorAndArgsCombinations_Data), MemberType = typeof(MemberSpecificationBuilderTests))]
            public void Should_AddValidRelativeRule(string message, IMessageArg[] args)
            {
                Predicate<object> isValid = c => true;

                var builder = new MemberSpecificationBuilder<object, object>();

                builder.ValidRelative(isValid, message, args);

                Assert.IsType<ValidRelativeRule<object>>(builder.Rules.Single());

                var memberRule = (ValidRelativeRule<object>)builder.Rules.Single();

                Assert.Same(isValid, memberRule.IsValid);

                if (message != null)
                {
                    Assert.Equal(message, memberRule.Error.Message);
                    Assert.Same(args, memberRule.Error.Arguments);
                }
                else
                {
                    Assert.Null(memberRule.Error);
                }
            }

            [Fact]
            public void Should_ThrowException_When_ValidRelative_And_ArgsWithoutMessage()
            {
                var builder = new MemberSpecificationBuilder<object, int?>();

                Assert.Throws<ArgumentException>(() => { builder.ValidRelative(c => true, null, new IMessageArg[] {new NumberArg("test", 1)}); });
            }

            [Fact]
            public void Should_ThrowException_When_ValidRelative_And_NullPredicate()
            {
                var builder = new MemberSpecificationBuilder<object, int?>();

                Assert.Throws<ArgumentNullException>(() => { builder.ValidRelative(null, "test"); });
            }
        }

        public class ValidNullable
        {
            [Fact]
            public void Should_AddValidNullableRule_When_ValidNullable()
            {
                var builder = new MemberSpecificationBuilder<object, int?>();

                MemberSpecification<object, int> memberSpecification = c => c;

                builder.ValidNullable(memberSpecification);

                Assert.IsType<ValidNullableRule<object, int>>(builder.Rules.Single());

                var validNullableRule = (ValidNullableRule<object, int>)builder.Rules.Single();

                Assert.Same(memberSpecification, validNullableRule.MemberSpecification);
            }

            [Fact]
            public void Should_ThrowException_When_ValidNullable_And_NullMemberSpecification()
            {
                var builder = new MemberSpecificationBuilder<object, int?>();

                Assert.Throws<ArgumentNullException>(() => { builder.ValidNullable(null); });
            }
        }

        public class ValidModel
        {
            [Fact]
            public void Should_AddValidModelRule_WithoutSpecification_When_ValidModel_And_NullSpecification()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                builder.ValidModel();

                Assert.IsType<ValidModelRule<object>>(builder.Rules.Single());

                var validModelRule = (ValidModelRule<object>)builder.Rules.Single();

                Assert.Null(validModelRule.Specification);
            }

            [Fact]
            public void Should_AddValidModelRule_WithSpecification_When_ValidModel()
            {
                Specification<object> specification = c => c;

                var builder = new MemberSpecificationBuilder<object, object>();

                builder.ValidModel(specification);

                Assert.IsType<ValidModelRule<object>>(builder.Rules.Single());

                var validModelRule = (ValidModelRule<object>)builder.Rules.Single();

                Assert.Same(specification, validModelRule.Specification);
            }
        }

        public class ValidCollection
        {
            public static IEnumerable<object[]> Should_AddValidCollectionRule_When_ValidModelsCollection_Data()
            {
                yield return new object[] {new Specification<object>(c => c), true};
                yield return new object[] {new Specification<object>(c => c), false};

                yield return new object[] {null, true};
                yield return new object[] {null, false};
            }

            [Theory]
            [MemberData(nameof(Should_AddValidCollectionRule_When_ValidModelsCollection_Data))]
            public void Should_AddValidCollectionRule_When_ValidModelsCollection(Specification<object> specification, bool isOptional)
            {
                var builder = new MemberSpecificationBuilder<object, IEnumerable<object>>();

                builder.ValidModelsCollection<object, IEnumerable<object>, object>(specification, isOptional);

                Assert.IsType<ValidCollectionRule<object, object>>(builder.Rules.Single());

                var validCollectionRule = (ValidCollectionRule<object, object>)builder.Rules.Single();

                Assert.NotNull(validCollectionRule.MemberSpecification);

                var processed = validCollectionRule.MemberSpecification(new MemberSpecificationBuilder<object, object>()) as MemberSpecificationBuilder<object, object>;

                Assert.NotNull(processed);
                Assert.IsType<ValidModelRule<object>>(processed.Rules.Single());
                Assert.Equal(isOptional, processed.IsOptional);

                var validModelRule = (ValidModelRule<object>)processed.Rules.Single();

                Assert.Same(specification, validModelRule.Specification);
            }

            [Fact]
            public void Should_AddValidCollectionRule_When_ValidCollection()
            {
                var builder = new MemberSpecificationBuilder<object, IEnumerable<object>>();

                MemberSpecification<object, object> memberSpecification = c => c;

                builder.ValidCollection<object, IEnumerable<object>, object>(memberSpecification);

                Assert.IsType<ValidCollectionRule<object, object>>(builder.Rules.Single());

                var validCollectionRule = (ValidCollectionRule<object, object>)builder.Rules.Single();

                Assert.Equal(memberSpecification, validCollectionRule.MemberSpecification);
            }
        }


        public class Optional
        {
            [Fact]
            public void Should_SetOptional()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                builder.Optional();

                Assert.True(builder.IsOptional);
            }

            [Fact]
            public void Should_ThrowException_When_DuplicateOptional()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Throws<InvalidOperationException>(() => { builder.Optional().Optional(); });
            }
        }

        [Fact]
        public void Should_HaveEmptyInitialValues()
        {
            var builder = new MemberSpecificationBuilder<object, object>();

            Assert.False(builder.IsOptional);
            Assert.Null(builder.Name);
            Assert.Null(builder.RequiredError);
            Assert.Null(builder.SummaryError);
            Assert.Empty(builder.Rules);
        }
    }
}