using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using CoreValidation.Validators;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Commands
{
    public class AsModelRuleTests
    {
        private static (AsModelRule, IValidatorsFactory) GetRule<T>(bool useRepository, Specification<T> specification)
            where T : class
        {
            var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

            if (useRepository)
            {
                specificationsRepositoryMock
                    .Setup(be => be.Get<T>())
                    .Returns(specification);
            }

            return (new AsModelRule<T>(useRepository ? null : specification), new ValidatorsFactory(specificationsRepositoryMock.Object));
        }

        public static IEnumerable<object[]> AllStrategies_NullAndObject_Data()
        {
            foreach (var validationStrategy in Enum.GetValues(typeof(ValidationStrategy)))
            {
                yield return new[] {validationStrategy, null};
                yield return new[] {validationStrategy, new object()};
            }
        }

        public static IEnumerable<object[]> Strategies_TrueAndFalse_Data(params ValidationStrategy[] strategies)
        {
            foreach (var validationStrategy in strategies)
            {
                yield return new object[] {validationStrategy, true};
                yield return new object[] {validationStrategy, false};
            }
        }

        public class AddingErrors
        {
            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, MemberType = typeof(AsModelRuleTests))]
            public void Should_AddErrorFromSpecification_When_Invalid(ValidationStrategy validationStrategy, bool useRepository)
            {
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => false).WithMessage("message"));

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = repository
                    },
                    validationStrategy,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, MemberType = typeof(AsModelRuleTests))]
            public void Should_NotAddErrorFromSpecification_When_Valid(ValidationStrategy validationStrategy, bool useRepository)
            {
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => true).WithMessage("message"));

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = repository
                    },
                    validationStrategy,
                    0,
                    out var errorsCollection);

                Assert.False(getErrorsResult);
                Assert.True(errorsCollection.IsEmpty);
            }

            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Force}, MemberType = typeof(AsModelRuleTests))]
            public void Should_AddErrorFromSpecification_When_Valid_And_Force(ValidationStrategy validationStrategy, bool useRepository)
            {
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => true).WithMessage("message"));

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = repository
                    },
                    validationStrategy,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }
        }

        public class PassingValuesToSpecification
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_PassMemberToRepoSpecification_When_NullSpecification(ValidationStrategy validationStrategy)
            {
                var member = new object();
                var executedRepoSpecification = 0;
                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c => c.Valid(m =>
                    {
                        Assert.Same(m, member);
                        executedRepoSpecification++;

                        return true;
                    }));

                AsModelRule rule = new AsModelRule<object>();

                rule.TryGetErrors(
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0,
                    out _);

                Assert.Equal(1, executedRepoSpecification);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_PassMemberToDefinedSpecification_When_SpecificationDefined(ValidationStrategy validationStrategy)
            {
                var member = new object();

                var executedRepoSpecification = 0;
                var executedDefinedSpecification = 0;
                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoSpecification++;

                        return c;
                    });

                Specification<object> specification = c => c.Valid(m =>
                {
                    Assert.Same(m, member);
                    executedDefinedSpecification++;

                    return true;
                });

                AsModelRule rule = new AsModelRule<object>(specification);

                rule.TryGetErrors(
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0,
                    out _);

                Assert.Equal(1, executedDefinedSpecification);
                Assert.Equal(0, executedRepoSpecification);
            }
        }

        public class ExecutingSpecifications
        {
            [Theory]
            [MemberData(nameof(AllStrategies_NullAndObject_Data), MemberType = typeof(AsModelRuleTests))]
            public void Should_ExecuteDefinedSpecification_When_SpecificationDefined(ValidationStrategy validationStrategy, object member)
            {
                var executedRepoSpecification = 0;
                var executedDefinedSpecification = 0;
                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoSpecification++;

                        return c;
                    });

                Specification<object> specification = c =>
                {
                    executedDefinedSpecification++;

                    return c;
                };

                AsModelRule rule = new AsModelRule<object>(specification);

                rule.TryGetErrors(
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0,
                    out _);

                Assert.Equal(1, executedDefinedSpecification);
                Assert.Equal(0, executedRepoSpecification);
            }

            [Theory]
            [MemberData(nameof(AllStrategies_NullAndObject_Data), MemberType = typeof(AsModelRuleTests))]
            public void Should_ExecuteRepositorySpecification_When_NullSpecification(ValidationStrategy validationStrategy, object member)
            {
                var executedRepoSpecification = 0;

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoSpecification++;

                        return c;
                    });

                AsModelRule rule = new AsModelRule<object>();

                rule.TryGetErrors(
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0,
                    out _);

                Assert.Equal(1, executedRepoSpecification);
            }
        }

        public class ExceedingMaxDepth
        {
            [Theory]
            [InlineData(0, true)]
            [InlineData(1, true)]
            [InlineData(2, true)]
            [InlineData(3, false)]
            [InlineData(4, false)]
            public void Should_AllowOnlyForMaxDepth_When_UsingRepositorySpecifications(int maxDepth, bool expectException)
            {
                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<Item1>())
                    .Returns(c => c.Member(m => m.Item2, m => m.AsModel()));

                specificationsRepositoryMock
                    .Setup(r => r.Get<Item2>())
                    .Returns(c => c.Member(m => m.Item3, m => m.AsModel()));

                specificationsRepositoryMock
                    .Setup(r => r.Get<Item3>())
                    .Returns(c => c.Valid(m => true));

                AsModelRule rule = new AsModelRule<Item1>();

                Action compilation = () =>
                {
                    rule.TryGetErrors(
                        new Item1(),
                        new ExecutionContextStub
                        {
                            MaxDepth = maxDepth,
                            ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                        },
                        ValidationStrategy.Complete,
                        0,
                        out _);
                };

                if (expectException)
                {
                    Assert.Throws<MaxDepthExceededException>(compilation);
                }
                else
                {
                    compilation();
                }
            }

            [Theory]
            [InlineData(0, true)]
            [InlineData(1, true)]
            [InlineData(2, true)]
            [InlineData(3, false)]
            [InlineData(4, false)]
            public void Should_AllowOnlyForMaxDepth_When_UsingDefinedSpecifications(int maxDepth, bool expectException)
            {
                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                Specification<Item3> item3Specification = c => c.Valid(m => false);
                Specification<Item2> item2Specification = c => c.Member(m => m.Item3, m => m.AsModel(item3Specification));
                Specification<Item1> item1Specification = c => c.Member(m => m.Item2, m => m.AsModel(item2Specification));

                AsModelRule rule = new AsModelRule<Item1>(item1Specification);

                Action compilation = () =>
                {
                    rule.TryGetErrors(
                        new Item1(),
                        new ExecutionContextStub
                        {
                            MaxDepth = maxDepth,
                            ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                        },
                        ValidationStrategy.Complete,
                        0,
                        out _);
                };

                if (expectException)
                {
                    Assert.Throws<MaxDepthExceededException>(compilation);
                }
                else
                {
                    compilation();
                }
            }


            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(5)]
            [InlineData(15)]
            public void Should_AllowOnlyForMaxDepth_When_Loop(int maxDepth)
            {
                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<ItemLoop>())
                    .Returns(c => c.Member(m => m.Nested, m => m.AsModel()).Valid(m => true));

                AsModelRule rule = new AsModelRule<ItemLoop>();

                Assert.Throws<MaxDepthExceededException>(() =>
                {
                    rule.TryGetErrors(
                        new ItemLoop(),
                        new ExecutionContextStub
                        {
                            MaxDepth = maxDepth,
                            ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                        },
                        ValidationStrategy.Complete,
                        0,
                        out _);
                });
            }


            public class Item1
            {
                public Item2 Item2 { get; } = new Item2();
            }

            public class Item2
            {
                public Item3 Item3 { get; } = new Item3();
            }

            public class Item3
            {
            }

            public class ItemLoop
            {
                public ItemLoop()
                {
                    Nested = this;
                }

                public ItemLoop Nested { get; }
            }
        }

        public class SingleRule
        {
            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, MemberType = typeof(AsModelRuleTests))]
            public void Should_AddRuleSingleError_When_Invalid(ValidationStrategy validationStrategy, bool useRepository)
            {
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => false));

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = repository
                    },
                    validationStrategy,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("ruleSingleError", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, MemberType = typeof(AsModelRuleTests))]
            public void Should_AddRuleSingleError_When_Invalid_MultipleRules(ValidationStrategy validationStrategy, bool useRepository)
            {
                var (rule, repository) = GetRule<object>(useRepository, be => be
                    .Valid(m => false).WithMessage("error1")
                    .Valid(m => false).WithMessage("error2")
                    .Valid(m => false).WithMessage("error3")
                );

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = repository
                    },
                    validationStrategy,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("ruleSingleError", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, MemberType = typeof(AsModelRuleTests))]
            public void Should_NotAddRuleSingleError_When_Valid(ValidationStrategy validationStrategy, bool useRepository)
            {
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => true));

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = repository
                    },
                    validationStrategy,
                    0,
                    out var errorsCollection);

                Assert.False(getErrorsResult);
                Assert.True(errorsCollection.IsEmpty);
            }

            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Force}, MemberType = typeof(AsModelRuleTests))]
            public void Should_AddRuleSingleError_When_Invalid_And_Force(ValidationStrategy validationStrategy, bool useRepository)
            {
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => true));

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = repository
                    },
                    validationStrategy,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("ruleSingleError", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }
        }
    }
}