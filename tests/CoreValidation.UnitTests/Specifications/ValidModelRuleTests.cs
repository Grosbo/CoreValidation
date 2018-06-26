using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors.Args;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;
using CoreValidation.Validators;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class ValidModelRuleTests
    {
        private static (ValidModelRule<T>, IValidatorsRepository) GetRule<T>(bool useRepository, Validator<T> validator)
            where T : class
        {
            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            if (useRepository)
            {
                validatorsRepositoryMock
                    .Setup(be => be.Get<T>())
                    .Returns(validator);
            }

            return (new ValidModelRule<T>(useRepository ? null : validator), validatorsRepositoryMock.Object);
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
            public void Should_AddErrorFromValidator_When_Invalid(ValidationStrategy validationStrategy, bool useRepository)
            {
                var args = new[] {new MessageArg("key", "value")};
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => false, "message", args));

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    repository,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Same(args, errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, MemberType = typeof(ValidModelRuleTests))]
            public void Should_NotAddErrorFromValidator_When_Valid(ValidationStrategy validationStrategy, bool useRepository)
            {
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => true, "message", new[] {new MessageArg("key", "value")}));

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    repository,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.False(errorsCollection.Errors.Any());
            }

            [Theory]
            [MemberData(nameof(Strategies_TrueAndFalse_Data), new[] {ValidationStrategy.Force}, MemberType = typeof(ValidModelRuleTests))]
            public void Should_NotAddErrorFromValidator_When_Valid_And_Force(ValidationStrategy validationStrategy, bool useRepository)
            {
                var args = new[] {new MessageArg("key", "value")};
                var (rule, repository) = GetRule<object>(useRepository, be => be.Valid(m => true, "message", args));

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    repository,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Same(args, errorsCollection.Errors.Single().Arguments);
            }
        }

        public class PassingValuesToValidator
        {
            [Theory]
            [MemberData(nameof(AllStrategies_NullAndObject_Data), MemberType = typeof(ValidModelRuleTests))]
            public void Should_PassMemberToRepoValidator_When_NullValidator(ValidationStrategy validationStrategy, object member)
            {
                var executedRepoValidator = 0;
                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        Assert.IsType<Specification<object>>(c);
                        Assert.Same(((Specification<object>)c).Model, member);
                        Assert.Equal(((Specification<object>)c).ValidationStrategy, validationStrategy);

                        executedRepoValidator++;

                        return c;
                    });

                var rule = new ValidModelRule<object>();

                rule.Compile(new[]
                {
                    member,
                    validatorsRepositoryMock.Object,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(1, executedRepoValidator);
            }

            [Theory]
            [MemberData(nameof(AllStrategies_NullAndObject_Data), MemberType = typeof(ValidModelRuleTests))]
            public void Should_PassMemberToDefinedValidator_When_ValidatorDefined(ValidationStrategy validationStrategy, object member)
            {
                var executedRepoValidator = 0;
                var executedDefinedValidator = 0;
                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoValidator++;

                        return c;
                    });

                Validator<object> validator = c =>
                {
                    Assert.IsType<Specification<object>>(c);
                    Assert.Same(((Specification<object>)c).Model, member);
                    Assert.Equal(((Specification<object>)c).ValidationStrategy, validationStrategy);

                    executedDefinedValidator++;

                    return c;
                };

                var rule = new ValidModelRule<object>(validator);

                rule.Compile(new[]
                {
                    member,
                    validatorsRepositoryMock.Object,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(1, executedDefinedValidator);
                Assert.Equal(0, executedRepoValidator);
            }
        }

        public class ExecutingValidators
        {
            [Theory]
            [MemberData(nameof(AllStrategies_NullAndObject_Data), MemberType = typeof(ValidModelRuleTests))]
            public void Should_ExecuteDefinedValidator_When_ValidatorDefined(ValidationStrategy validationStrategy, object member)
            {
                var executedRepoValidator = 0;
                var executedDefinedValidator = 0;
                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoValidator++;

                        return c;
                    });

                Validator<object> validator = c =>
                {
                    executedDefinedValidator++;

                    return c;
                };

                var rule = new ValidModelRule<object>(validator);

                rule.Compile(new[]
                {
                    member,
                    validatorsRepositoryMock.Object,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(1, executedDefinedValidator);
                Assert.Equal(0, executedRepoValidator);
            }

            [Theory]
            [MemberData(nameof(AllStrategies_NullAndObject_Data), MemberType = typeof(ValidModelRuleTests))]
            public void Should_ExecuteRepositoryValidator_When_NullValidator(ValidationStrategy validationStrategy, object member)
            {
                var executedRepoValidator = 0;

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoValidator++;

                        return c;
                    });

                var rule = new ValidModelRule<object>();

                rule.Compile(new[]
                {
                    member,
                    validatorsRepositoryMock.Object,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(1, executedRepoValidator);
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
            public void Should_AllowOnlyForMaxDepth_When_UsingRepositoryValidators(int maxDepth, bool expectException)
            {
                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<Item1>())
                    .Returns(c => c.For(m => m.Item2, m => m.ValidModel()));

                validatorsRepositoryMock
                    .Setup(r => r.Get<Item2>())
                    .Returns(c => c.For(m => m.Item3, m => m.ValidModel()));

                validatorsRepositoryMock
                    .Setup(r => r.Get<Item3>())
                    .Returns(c => c.Valid(m => true, "error"));

                var rule = new ValidModelRule<Item1>();

                Action compilation = () =>
                {
                    rule.Compile(new object[]
                    {
                        new Item1(),
                        validatorsRepositoryMock.Object,
                        ValidationStrategy.Complete,
                        0,
                        new RulesOptionsStub
                        {
                            MaxDepth = maxDepth
                        }
                    });
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
            public void Should_AllowOnlyForMaxDepth_When_UsingDefinedValidators(int maxDepth, bool expectException)
            {
                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                Validator<Item3> item3Validator = c => c.Valid(m => false, "error");
                Validator<Item2> item2Validator = c => c.For(m => m.Item3, m => m.ValidModel(item3Validator));
                Validator<Item1> item1Validator = c => c.For(m => m.Item2, m => m.ValidModel(item2Validator));

                var rule = new ValidModelRule<Item1>(item1Validator);

                Action compilation = () =>
                {
                    rule.Compile(new object[]
                    {
                        new Item1(),
                        validatorsRepositoryMock.Object,
                        ValidationStrategy.Complete,
                        0,
                        new RulesOptionsStub
                        {
                            MaxDepth = maxDepth
                        }
                    });
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
                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<ItemLoop>())
                    .Returns(c => c.For(m => m.Nested, m => m.ValidModel()).Valid(m => true, "error"));

                var rule = new ValidModelRule<ItemLoop>();

                Assert.Throws<MaxDepthExceededException>(() =>
                {
                    rule.Compile(new object[]
                    {
                        new ItemLoop(),
                        validatorsRepositoryMock.Object,
                        ValidationStrategy.Complete,
                        0,
                        new RulesOptionsStub
                        {
                            MaxDepth = maxDepth
                        }
                    });
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