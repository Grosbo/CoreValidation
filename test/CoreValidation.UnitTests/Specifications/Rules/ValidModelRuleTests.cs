using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors.Args;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Rules;
using CoreValidation.Validators;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Rules
{
    public class ValidModelRuleTests
    {
        private static (ValidModelRule, IValidatorsFactory) GetRule<T>(bool useRepository, Specification<T> specification)
            where T : class
        {
            var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

            if (useRepository)
            {
                specificationsRepositoryMock
                    .Setup(be => be.Get<T>())
                    .Returns(specification);
            }

            return (new ValidModelRule<T>(useRepository ? null : specification), new ValidatorsFactory(specificationsRepositoryMock.Object));
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
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, MemberType = typeof(ValidModelRuleTests))]
            public void Should_AddErrorFromSpecification_When_Invalid(ValidationStrategy validationStrategy, bool useRepository)
            {
                var args = new[] {new MessageArg("key", "value")};
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => false, "message", args));

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    new ExecutionContext
                    {
                        ExecutionOptions = new ExecutionOptionsStub(),
                        ValidatorsFactory = repository,
                        ValidationStrategy = validationStrategy
                    },
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Same(args, errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, MemberType = typeof(ValidModelRuleTests))]
            public void Should_NotAddErrorFromSpecification_When_Valid(ValidationStrategy validationStrategy, bool useRepository)
            {
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => true, "message", new[] {new MessageArg("key", "value")}));

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    new ExecutionContext
                    {
                        ExecutionOptions = new ExecutionOptionsStub(),
                        ValidatorsFactory = repository,
                        ValidationStrategy = validationStrategy
                    },
                    0,
                    out var errorsCollection);

                Assert.False(getErrorsResult);
                Assert.True(errorsCollection.IsEmpty);
            }

            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Force}, MemberType = typeof(ValidModelRuleTests))]
            public void Should_AddErrorFromSpecification_When_Valid_And_Force(ValidationStrategy validationStrategy, bool useRepository)
            {
                var args = new[] {new MessageArg("key", "value")};
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => true, "message", args));

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    new ExecutionContext
                    {
                        ExecutionOptions = new ExecutionOptionsStub(),
                        ValidatorsFactory = repository,
                        ValidationStrategy = validationStrategy
                    },
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Same(args, errorsCollection.Errors.Single().Arguments);
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

                ValidModelRule rule = new ValidModelRule<object>();

                rule.TryGetErrors(
                    member,
                    new ExecutionContext
                    {
                        ExecutionOptions = new ExecutionOptionsStub(),
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object),
                        ValidationStrategy = validationStrategy
                    },
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

                ValidModelRule rule = new ValidModelRule<object>(specification);

                rule.TryGetErrors(
                    member,
                    new ExecutionContext
                    {
                        ExecutionOptions = new ExecutionOptionsStub(),
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object),
                        ValidationStrategy = validationStrategy
                    },
                    0,
                    out _);

                Assert.Equal(1, executedDefinedSpecification);
                Assert.Equal(0, executedRepoSpecification);
            }
        }

        public class ExecutingSpecifications
        {
            [Theory]
            [MemberData(nameof(AllStrategies_NullAndObject_Data), MemberType = typeof(ValidModelRuleTests))]
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

                ValidModelRule rule = new ValidModelRule<object>(specification);

                rule.TryGetErrors(
                    member,
                    new ExecutionContext
                    {
                        ExecutionOptions = new ExecutionOptionsStub(),
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object),
                        ValidationStrategy = validationStrategy
                    },
                    0,
                    out _);

                Assert.Equal(1, executedDefinedSpecification);
                Assert.Equal(0, executedRepoSpecification);
            }

            [Theory]
            [MemberData(nameof(AllStrategies_NullAndObject_Data), MemberType = typeof(ValidModelRuleTests))]
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

                ValidModelRule rule = new ValidModelRule<object>();

                rule.TryGetErrors(
                    member,
                    new ExecutionContext
                    {
                        ExecutionOptions = new ExecutionOptionsStub(),
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object),
                        ValidationStrategy = validationStrategy
                    },
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
                    .Returns(c => c.For(m => m.Item2, m => m.ValidModel()));

                specificationsRepositoryMock
                    .Setup(r => r.Get<Item2>())
                    .Returns(c => c.For(m => m.Item3, m => m.ValidModel()));

                specificationsRepositoryMock
                    .Setup(r => r.Get<Item3>())
                    .Returns(c => c.Valid(m => true, "error"));

                ValidModelRule rule = new ValidModelRule<Item1>();

                Action compilation = () =>
                {
                    rule.TryGetErrors(
                        new Item1(),
                        new ExecutionContext
                        {
                            ExecutionOptions = new ExecutionOptionsStub
                            {
                                MaxDepth = maxDepth
                            },
                            ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object),
                            ValidationStrategy = ValidationStrategy.Complete
                        },
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

                Specification<Item3> item3Specification = c => c.Valid(m => false, "error");
                Specification<Item2> item2Specification = c => c.For(m => m.Item3, m => m.ValidModel(item3Specification));
                Specification<Item1> item1Specification = c => c.For(m => m.Item2, m => m.ValidModel(item2Specification));

                ValidModelRule rule = new ValidModelRule<Item1>(item1Specification);

                Action compilation = () =>
                {
                    rule.TryGetErrors(
                        new Item1(),
                        new ExecutionContext
                        {
                            ExecutionOptions = new ExecutionOptionsStub
                            {
                                MaxDepth = maxDepth
                            },
                            ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object),
                            ValidationStrategy = ValidationStrategy.Complete
                        },
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
                    .Returns(c => c.For(m => m.Nested, m => m.ValidModel()).Valid(m => true));

                ValidModelRule rule = new ValidModelRule<ItemLoop>();

                Assert.Throws<MaxDepthExceededException>(() =>
                {
                    rule.TryGetErrors(
                        new ItemLoop(),
                        new ExecutionContext
                        {
                            ExecutionOptions = new ExecutionOptionsStub
                            {
                                MaxDepth = maxDepth
                            },
                            ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object),
                            ValidationStrategy = ValidationStrategy.Complete
                        },
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
    }
}