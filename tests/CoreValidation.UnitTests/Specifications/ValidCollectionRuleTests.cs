using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;
using CoreValidation.Validators;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class ValidCollectionRuleTests
    {
        public static IEnumerable<object[]> AllCollections_With_InvalidItems_Data()
        {
            var invalidItem = new object();

            yield return new[] {new[] {new object()}, invalidItem};
            yield return new[] {new[] {new object(), invalidItem}, invalidItem};
            yield return new[] {new[] {new object(), new object(), new object()}, invalidItem};
            yield return new[] {new[] {new object(), null, new object()}, invalidItem};
            yield return new[] {new[] {new object(), new object(), invalidItem}, invalidItem};
            yield return new[] {new[] {invalidItem, invalidItem}, invalidItem};
            yield return new[] {new[] {null, invalidItem}, invalidItem};
            yield return new[] {new[] {new object(), null, invalidItem}, invalidItem};
            yield return new[] {new[] {new object(), invalidItem, null}, invalidItem};
            yield return new[] {new object[] {null}, invalidItem};
        }

        public static IEnumerable<object[]> AllCollections()
        {
            yield return new object[] {new[] {new object()}};
            yield return new object[] {new[] {new object(), new object(), new object()}};
            yield return new object[] {new[] {new object(), null, new object()}};
            yield return new object[] {new object[] {null}};
            yield return new object[] {null};
        }

        public static IEnumerable<object[]> Strategies_AllCollections(params ValidationStrategy[] strategies)
        {
            foreach (var strategy in strategies)
            {
                yield return new object[] {strategy, new[] {new object()}};
                yield return new object[] {strategy, new[] {new object(), new object(), new object()}};
                yield return new object[] {strategy, new[] {new object(), null, new object()}};
                yield return new object[] {strategy, new object[] {null}};
                yield return new object[] {strategy, null};
            }
        }

        public static IReadOnlyCollection<int> GetNotNullIndexes<T>(IEnumerable<T> member)
        {
            if (member == null)
            {
                return new int[] { };
            }

            var indexes = new List<int>();

            var enumerable = member as T[] ?? member.ToArray();

            for (var i = 0; i < enumerable.Count(); ++i)
            {
                if (enumerable.ElementAt(i) != null)
                {
                    indexes.Add(i);
                }
            }

            return indexes;
        }

        public class AddingMultipleErrorsToOutputCollection
        {
            [Fact]
            public void Should_AddFirstErrorToFirstItem_When_FailFast()
            {
                var invalidItem = new object();

                var member = new[]
                {
                    new object(),
                    new object(),
                    invalidItem,
                    new object(),
                    invalidItem
                };

                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .Valid(v => v != invalidItem, "invalidItem2")
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.FailFast,
                    0,
                    new RulesOptionsStub()
                });

                Assert.False(errorsCollection.IsEmpty);

                Assert.Equal(1, errorsCollection.Members.Count);

                Assert.Equal(1, errorsCollection.Members["2"].Errors.Count);
                Assert.Equal("invalidItem", errorsCollection.Members["2"].Errors.ElementAt(0).Message);
            }

            [Fact]
            public void Should_AddManyErrorsToItem_When_Complete()
            {
                var invalidItem = new object();

                var member = new[]
                {
                    new object(),
                    new object(),
                    invalidItem,
                    new object(),
                    invalidItem
                };

                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .Valid(v => v != invalidItem, "invalidItem2")
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Complete,
                    0,
                    new RulesOptionsStub()
                });

                Assert.False(errorsCollection.IsEmpty);

                Assert.Equal(2, errorsCollection.Members.Count);

                Assert.Equal(2, errorsCollection.Members["2"].Errors.Count);
                Assert.Equal("invalidItem", errorsCollection.Members["2"].Errors.ElementAt(0).Message);
                Assert.Equal("invalidItem2", errorsCollection.Members["2"].Errors.ElementAt(1).Message);

                Assert.Equal(2, errorsCollection.Members["4"].Errors.Count);
                Assert.Equal("invalidItem", errorsCollection.Members["4"].Errors.ElementAt(0).Message);
                Assert.Equal("invalidItem2", errorsCollection.Members["4"].Errors.ElementAt(1).Message);
            }

            [Fact]
            public void Should_AddManyErrorsToItem_When_Force()
            {
                var invalidItem = new object();

                var member = new[]
                {
                    new object(),
                    new object(),
                    invalidItem,
                    new object(),
                    invalidItem
                };

                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .Valid(v => v != invalidItem, "invalidItem2")
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Force,
                    0,
                    new RulesOptionsStub()
                });

                Assert.False(errorsCollection.IsEmpty);

                Assert.Equal(1, errorsCollection.Members.Count);

                Assert.Equal(3, errorsCollection.Members["*"].Errors.Count);
                Assert.Equal("Required", errorsCollection.Members["*"].Errors.ElementAt(0).Message);
                Assert.Equal("invalidItem", errorsCollection.Members["*"].Errors.ElementAt(1).Message);
                Assert.Equal("invalidItem2", errorsCollection.Members["*"].Errors.ElementAt(2).Message);
            }
        }

        public class AddingErrorsToOutputCollection
        {
            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_AddErrors_When_Complete(object[] member, object invalidItem)
            {
                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Complete,
                    0,
                    new RulesOptionsStub()
                });

                Assert.NotNull(errorsCollection);

                if (member == null)
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else if (!member.Contains(invalidItem) && !member.Contains(null))
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.False(errorsCollection.IsEmpty);

                    Assert.Equal(member.Count(m => (m == invalidItem) || (m == null)), errorsCollection.Members.Count);

                    for (var i = 0; i < member.Length; ++i)
                    {
                        var item = member.ElementAt(i);

                        if (item == null)
                        {
                            Assert.Equal("Required", errorsCollection.Members[i.ToString()].Errors.Single().Message);
                        }
                        else if (item == invalidItem)
                        {
                            Assert.Equal("invalidItem", errorsCollection.Members[i.ToString()].Errors.Single().Message);
                        }
                        else
                        {
                            Assert.False(errorsCollection.Members.ContainsKey(i.ToString()));
                        }
                    }
                }
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_AddErrors_When_Complete_And_WithSummaryError(object[] member, object invalidItem)
            {
                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .WithSummaryError("invalidItem_overriden")
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Complete,
                    0,
                    new RulesOptionsStub()
                });

                Assert.NotNull(errorsCollection);

                if (member == null)
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else if (!member.Contains(invalidItem) && !member.Contains(null))
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.False(errorsCollection.IsEmpty);

                    Assert.Equal(member.Count(m => (m == invalidItem) || (m == null)), errorsCollection.Members.Count);

                    for (var i = 0; i < member.Length; ++i)
                    {
                        var item = member.ElementAt(i);

                        if (item == null)
                        {
                            Assert.Equal("Required", errorsCollection.Members[i.ToString()].Errors.Single().Message);
                        }
                        else if (item == invalidItem)
                        {
                            Assert.Equal("invalidItem_overriden", errorsCollection.Members[i.ToString()].Errors.Single().Message);
                        }
                        else
                        {
                            Assert.False(errorsCollection.Members.ContainsKey(i.ToString()));
                        }
                    }
                }
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_AddErrors_When_Complete_And_Optional(object[] member, object invalidItem)
            {
                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .Optional()
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Complete,
                    0,
                    new RulesOptionsStub()
                });

                Assert.NotNull(errorsCollection);

                if (member == null)
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else if (!member.Contains(invalidItem))
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.False(errorsCollection.IsEmpty);

                    Assert.Equal(member.Count(m => m == invalidItem), errorsCollection.Members.Count);

                    for (var i = 0; i < member.Length; ++i)
                    {
                        var item = member.ElementAt(i);

                        if (item == invalidItem)
                        {
                            Assert.Equal("invalidItem", errorsCollection.Members[i.ToString()].Errors.Single().Message);
                        }
                        else
                        {
                            Assert.False(errorsCollection.Members.ContainsKey(i.ToString()));
                        }
                    }
                }
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_AddErrors_When_FailFast(object[] member, object invalidItem)
            {
                var model = new object();

                var firstInvalidIndex = -1;

                for (var i = 0; i < member.Length; ++i)
                {
                    if (member.ElementAt(i) == invalidItem)
                    {
                        firstInvalidIndex = i;

                        break;
                    }
                }

                var firstNullIndex = -1;

                for (var i = 0; i < member.Length; ++i)
                {
                    if (member.ElementAt(i) == null)
                    {
                        firstNullIndex = i;

                        break;
                    }
                }

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.FailFast,
                    0,
                    new RulesOptionsStub()
                });

                Assert.NotNull(errorsCollection);

                if (member == null)
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else if (!member.Contains(invalidItem) && !member.Contains(null))
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.False(errorsCollection.IsEmpty);
                    Assert.Single(errorsCollection.Members);

                    if ((firstInvalidIndex != -1) && (firstNullIndex == -1))
                    {
                        Assert.Equal("invalidItem", errorsCollection.Members[firstInvalidIndex.ToString()].Errors.Single().Message);
                    }
                    else if ((firstNullIndex != -1) && (firstInvalidIndex == -1))
                    {
                        Assert.Equal("Required", errorsCollection.Members[firstNullIndex.ToString()].Errors.Single().Message);
                    }
                    else
                    {
                        var firstErrorIndex = Math.Min(firstInvalidIndex, firstNullIndex);

                        Assert.Equal(firstInvalidIndex < firstNullIndex ? "invalidItem" : "Required", errorsCollection.Members[firstErrorIndex.ToString()].Errors.Single().Message);
                    }
                }
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_AddErrors_When_FailFast_And_Optional(object[] member, object invalidItem)
            {
                var model = new object();

                var firstInvalidIndex = -1;

                for (var i = 0; i < member.Length; ++i)
                {
                    if (member.ElementAt(i) == invalidItem)
                    {
                        firstInvalidIndex = i;

                        break;
                    }
                }

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .Optional()
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.FailFast,
                    0,
                    new RulesOptionsStub()
                });

                Assert.NotNull(errorsCollection);

                if (member == null)
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else if (!member.Contains(invalidItem))
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.False(errorsCollection.IsEmpty);
                    Assert.Single(errorsCollection.Members);
                    Assert.Equal("invalidItem", errorsCollection.Members[firstInvalidIndex.ToString()].Errors.Single().Message);
                }
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_AddErrors_When_FailFast_And_WithSummaryError(object[] member, object invalidItem)
            {
                var model = new object();

                var firstInvalidIndex = -1;

                for (var i = 0; i < member.Length; ++i)
                {
                    if (member.ElementAt(i) == invalidItem)
                    {
                        firstInvalidIndex = i;

                        break;
                    }
                }

                var firstNullIndex = -1;

                for (var i = 0; i < member.Length; ++i)
                {
                    if (member.ElementAt(i) == null)
                    {
                        firstNullIndex = i;

                        break;
                    }
                }

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .WithSummaryError("invalidItem_overriden")
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.FailFast,
                    0,
                    new RulesOptionsStub()
                });

                Assert.NotNull(errorsCollection);

                if (member == null)
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else if (!member.Contains(invalidItem) && !member.Contains(null))
                {
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.False(errorsCollection.IsEmpty);
                    Assert.Single(errorsCollection.Members);

                    if ((firstInvalidIndex != -1) && (firstNullIndex == -1))
                    {
                        Assert.Equal("invalidItem_overriden", errorsCollection.Members[firstInvalidIndex.ToString()].Errors.Single().Message);
                    }
                    else if ((firstNullIndex != -1) && (firstInvalidIndex == -1))
                    {
                        Assert.Equal("Required", errorsCollection.Members[firstNullIndex.ToString()].Errors.Single().Message);
                    }
                    else if (firstInvalidIndex < firstNullIndex)
                    {
                        Assert.Equal("invalidItem_overriden", errorsCollection.Members[firstInvalidIndex.ToString()].Errors.Single().Message);
                    }
                    else if (firstNullIndex < firstInvalidIndex)
                    {
                        Assert.Equal("Required", errorsCollection.Members[firstNullIndex.ToString()].Errors.Single().Message);
                    }
                }
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_AddErrors_When_Force(object[] member, object invalidItem)
            {
                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Force,
                    0,
                    new RulesOptionsStub()
                });

                Assert.False(errorsCollection.IsEmpty);

                Assert.Equal(1, errorsCollection.Members.Count);
                Assert.Equal(2, errorsCollection.Members["*"].Errors.Count);

                Assert.Equal("Required", errorsCollection.Members["*"].Errors.First().Message);
                Assert.Equal("invalidItem", errorsCollection.Members["*"].Errors.Last().Message);
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_AddErrors_When_Force_And_Optional(object[] member, object invalidItem)
            {
                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .Optional()
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Force,
                    0,
                    new RulesOptionsStub()
                });

                Assert.False(errorsCollection.IsEmpty);

                Assert.Equal(1, errorsCollection.Members.Count);
                Assert.Equal(1, errorsCollection.Members["*"].Errors.Count);

                Assert.Equal("invalidItem", errorsCollection.Members["*"].Errors.Last().Message);
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_AddErrors_When_Force_And_WithSummaryError(object[] member, object invalidItem)
            {
                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .WithSummaryError("invalidItem_overriden")
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Force,
                    0,
                    new RulesOptionsStub()
                });

                Assert.False(errorsCollection.IsEmpty);

                Assert.Equal(1, errorsCollection.Members.Count);
                Assert.Equal(2, errorsCollection.Members["*"].Errors.Count);

                Assert.Equal("Required", errorsCollection.Members["*"].Errors.ElementAt(0).Message);
                Assert.Equal("invalidItem_overriden", errorsCollection.Members["*"].Errors.ElementAt(1).Message);
            }


            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddCustomRequiredError_When_WithRequiredError(ValidationStrategy validationStrategy)
            {
                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(v => false, "invalidItem")
                    .WithRequiredError("Value is required (overriden)")
                );

                var errorsCollection = rule.Compile(new[]
                {
                    model,
                    new object[]
                    {
                        null
                    },
                    validatorsRepositoryMock.Object,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.False(errorsCollection.IsEmpty);
                Assert.Equal(1, errorsCollection.Members.Count);

                if ((validationStrategy == ValidationStrategy.Complete) ||
                    (validationStrategy == ValidationStrategy.FailFast))
                {
                    Assert.Equal("Value is required (overriden)", errorsCollection.Members["0"].Errors.Single().Message);
                }
                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.Equal(2, errorsCollection.Members["*"].Errors.Count);
                    Assert.Equal("Value is required (overriden)", errorsCollection.Members["*"].Errors.ElementAt(0).Message);
                    Assert.Equal("invalidItem", errorsCollection.Members["*"].Errors.ElementAt(1).Message);
                }
            }
        }

        public class PassingValuesToPredicates
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_PassMemberValueToPredicate(ValidationStrategy validationStrategy)
            {
                var model = new object();

                var member = Enumerable.Range(0, 10).ToList();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var executionCounter = 0;

                var rule = new ValidCollectionRule<object, int>(be => be
                    .Valid(m =>
                    {
                        Assert.Equal(member.ElementAt(executionCounter), m);

                        executionCounter++;

                        return true;
                    }, "message")
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(member.Count, executionCounter);
            }

            [Theory]
            [MemberData(nameof(Strategies_AllCollections), new[] {ValidationStrategy.Complete}, MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_PassMemberReferenceToPredicate(ValidationStrategy validationStrategy, object[] member)
            {
                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var nonNullIndexes = GetNotNullIndexes(member);

                var executionCounter = 0;

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(m =>
                    {
                        var nextNonNullIndex = nonNullIndexes.ElementAt(executionCounter);
                        Assert.Same(member.ElementAt(nextNonNullIndex), m);

                        executionCounter++;

                        return true;
                    }, "message")
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(nonNullIndexes.Count, executionCounter);
            }

            [Theory]
            [MemberData(nameof(Strategies_AllCollections), new[] {ValidationStrategy.Complete}, MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_PassModelReferenceToPredicate(ValidationStrategy validationStrategy, object[] member)
            {
                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var nonNullIndexes = GetNotNullIndexes(member);

                var executionCounter = 0;

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidRelative(m =>
                    {
                        Assert.Same(model, m);

                        executionCounter++;

                        return true;
                    }, "message")
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(nonNullIndexes.Count, executionCounter);
            }

            [Theory]
            [MemberData(nameof(Strategies_AllCollections), new[] {ValidationStrategy.Complete}, MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_PassMemberReferenceToNestedPredicate_When_ValidatorDefined(ValidationStrategy validationStrategy, object[] member)
            {
                var model = new object();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var nonNullIndexes = GetNotNullIndexes(member);

                var executionCounter = 0;

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidModel(n => n.Valid(m =>
                    {
                        var nextNonNullIndex = nonNullIndexes.ElementAt(executionCounter);
                        Assert.Same(member.ElementAt(nextNonNullIndex), m);

                        executionCounter++;

                        return true;
                    }, "message"))
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(nonNullIndexes.Count, executionCounter);
            }

            [Theory]
            [MemberData(nameof(Strategies_AllCollections), new[] {ValidationStrategy.Complete}, MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_PassMemberReferenceToNestedPredicate_When_ValidatorFromRepository(ValidationStrategy validationStrategy, object[] member)
            {
                var model = new object();

                var nonNullIndexes = GetNotNullIndexes(member);

                var executionCounter = 0;

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(n => n.Valid(m =>
                    {
                        var nextNonNullIndex = nonNullIndexes.ElementAt(executionCounter);
                        Assert.Same(member.ElementAt(nextNonNullIndex), m);

                        executionCounter++;

                        return true;
                    }, "message"));

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidModel()
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    validationStrategy,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(nonNullIndexes.Count, executionCounter);
            }
        }

        public class ExecutingRepositoryValidator
        {
            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_ExecuteRepositoryValidator_When_Complete(object[] member)
            {
                var model = new object();

                var executedRepoValidator = 0;

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoValidator++;

                        return c;
                    });

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidModel()
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Complete,
                    0,
                    new RulesOptionsStub()
                });

                var nonNullIndexes = GetNotNullIndexes(member);

                Assert.Equal(nonNullIndexes.Count, executedRepoValidator);
            }

            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_ExecuteRepositoryValidator_When_Force(object[] member)
            {
                var model = new object();

                var executedRepoValidator = 0;

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoValidator++;

                        return c;
                    });

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidModel()
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Force,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(1, executedRepoValidator);
            }

            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_ExecuteRepositoryValidator_When_ValidItemsWithNulls_FailFast(object[] member)
            {
                var model = new object();

                var firstNullIndex = -1;

                if (member != null)
                {
                    for (var i = 0; i < member.Length; ++i)
                    {
                        if (member.ElementAt(i) == null)
                        {
                            firstNullIndex = i;

                            break;
                        }
                    }
                }

                var executedRepoValidator = 0;

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoValidator++;

                        return c;
                    });

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidModel()
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.FailFast,
                    0,
                    new RulesOptionsStub()
                });

                if (firstNullIndex == -1)
                {
                    Assert.Equal(member?.Length ?? 0, executedRepoValidator);
                }
                else
                {
                    Assert.Equal(firstNullIndex, executedRepoValidator);
                }
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_ExecuteRepositoryValidator_When_FailFast(object[] member, object invalidItem)
            {
                var model = new object();

                var firstInvalidIndex = -1;

                for (var i = 0; i < member.Length; ++i)
                {
                    if (member.ElementAt(i) == invalidItem)
                    {
                        firstInvalidIndex = i;

                        break;
                    }
                }

                var firstNullIndex = -1;

                for (var i = 0; i < member.Length; ++i)
                {
                    if (member.ElementAt(i) == null)
                    {
                        firstNullIndex = i;

                        break;
                    }
                }

                var executedRepoValidator = 0;

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoValidator++;

                        return c.Valid(m => m != invalidItem, "error");
                    });

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidModel()
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.FailFast,
                    0,
                    new RulesOptionsStub()
                });

                if ((firstInvalidIndex == -1) && (firstNullIndex == -1))
                {
                    Assert.Equal(member.Length, executedRepoValidator);
                }
                else if ((firstInvalidIndex != -1) && (firstNullIndex == -1))
                {
                    Assert.Equal(firstInvalidIndex + 1, executedRepoValidator);
                }
                else if ((firstNullIndex != -1) && (firstInvalidIndex == -1))
                {
                    Assert.Equal(firstNullIndex, executedRepoValidator);
                }
                else
                {
                    Assert.Equal(Math.Min(firstInvalidIndex + 1, firstNullIndex), executedRepoValidator);
                }
            }
        }

        public class ExecutingDefinedValidator
        {
            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_ExecuteDefinedValidator_When_Complete(object[] member)
            {
                var model = new object();

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

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidModel(m =>
                    {
                        executedDefinedValidator++;

                        return m;
                    })
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Complete,
                    0,
                    new RulesOptionsStub()
                });

                var nonNullIndexes = GetNotNullIndexes(member);

                Assert.Equal(nonNullIndexes.Count, executedDefinedValidator);
                Assert.Equal(0, executedRepoValidator);
            }

            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_ExecuteDefinedValidator_When_Force(object[] member)
            {
                var model = new object();

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

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidModel(m =>
                    {
                        executedDefinedValidator++;

                        return m;
                    })
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Force,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(1, executedDefinedValidator);
                Assert.Equal(0, executedRepoValidator);
            }

            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_ExecuteDefinedValidator_When_ValidItemsWithNulls_FailFast(object[] member)
            {
                var model = new object();

                var firstNullIndex = -1;

                if (member != null)
                {
                    for (var i = 0; i < member.Length; ++i)
                    {
                        if (member.ElementAt(i) == null)
                        {
                            firstNullIndex = i;

                            break;
                        }
                    }
                }

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

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidModel(m =>
                    {
                        executedDefinedValidator++;

                        return m;
                    })
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.FailFast,
                    0,
                    new RulesOptionsStub()
                });

                if (firstNullIndex == -1)
                {
                    Assert.Equal(member?.Length ?? 0, executedDefinedValidator);
                }
                else
                {
                    Assert.Equal(firstNullIndex, executedDefinedValidator);
                }

                Assert.Equal(0, executedRepoValidator);
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(ValidCollectionRuleTests))]
            public void Should_ExecuteDefinedValidator_When_FailFast(object[] member, object invalidItem)
            {
                var model = new object();

                var firstInvalidIndex = -1;

                for (var i = 0; i < member.Length; ++i)
                {
                    if (member.ElementAt(i) == invalidItem)
                    {
                        firstInvalidIndex = i;

                        break;
                    }
                }

                var firstNullIndex = -1;

                for (var i = 0; i < member.Length; ++i)
                {
                    if (member.ElementAt(i) == null)
                    {
                        firstNullIndex = i;

                        break;
                    }
                }

                var executedRepoValidator = 0;
                var executedDefinedValidator = 0;

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoValidator++;

                        return c.Valid(m => m != invalidItem, "error");
                    });

                var rule = new ValidCollectionRule<object, object>(be => be
                    .ValidModel(m =>
                    {
                        executedDefinedValidator++;

                        return m.Valid(v => v != invalidItem, "error");
                    })
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.FailFast,
                    0,
                    new RulesOptionsStub()
                });

                if ((firstInvalidIndex == -1) && (firstNullIndex == -1))
                {
                    Assert.Equal(member.Length, executedDefinedValidator);
                }
                else if ((firstInvalidIndex != -1) && (firstNullIndex == -1))
                {
                    Assert.Equal(firstInvalidIndex + 1, executedDefinedValidator);
                }
                else if ((firstNullIndex != -1) && (firstInvalidIndex == -1))
                {
                    Assert.Equal(firstNullIndex, executedDefinedValidator);
                }
                else
                {
                    Assert.Equal(Math.Min(firstInvalidIndex + 1, firstNullIndex), executedDefinedValidator);
                }

                Assert.Equal(0, executedRepoValidator);
            }
        }

        public class ExecutingDifferentValidators
        {
            public class DualEnumerable : IEnumerable<object>, IEnumerable<int>
            {
                public readonly List<int> Integers = new List<int> {1, 2, 3};

                public readonly List<object> Objects = new List<object> {new object(), new object(), new object()};

                IEnumerator<int> IEnumerable<int>.GetEnumerator()
                {
                    return Integers.GetEnumerator();
                }

                IEnumerator<object> IEnumerable<object>.GetEnumerator()
                {
                    return Objects.GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return Objects.GetEnumerator();
                }
            }

            public class DualObjectsEnumerable : IEnumerable<ItemA>, IEnumerable<ItemB>
            {
                public readonly List<ItemA> ItemsA = new List<ItemA> {new ItemA(), new ItemA(), new ItemA()};

                public readonly List<ItemB> ItemsB = new List<ItemB> {new ItemB(), new ItemB(), new ItemB()};

                IEnumerator<ItemA> IEnumerable<ItemA>.GetEnumerator()
                {
                    return ItemsA.GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return ItemsA.GetEnumerator();
                }

                IEnumerator<ItemB> IEnumerable<ItemB>.GetEnumerator()
                {
                    return ItemsB.GetEnumerator();
                }
            }

            public class ItemA
            {
            }

            public class ItemB
            {
            }

            [Fact]
            public void Should_ExecuteIntegerDefinedValidator_When_MultipleEnumerators()
            {
                var model = new object();

                var dualEnumerable = new DualEnumerable();

                var executed = 0;

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, int>(be => be
                    .Valid(i =>
                    {
                        Assert.Equal(dualEnumerable.Integers.ElementAt(executed), i);

                        executed++;

                        return true;
                    }, "error"));

                rule.Compile(new[]
                {
                    model,
                    dualEnumerable,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Complete,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(3, executed);
            }

            [Fact]
            public void Should_ExecuteObjectDefinedValidator_When_MultipleEnumerators()
            {
                var model = new object();

                var dualEnumerable = new DualEnumerable();

                var executed = 0;

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rule = new ValidCollectionRule<object, object>(be => be
                    .Valid(i =>
                    {
                        Assert.Same(dualEnumerable.Objects.ElementAt(executed), i);

                        executed++;

                        return true;
                    }, "error"));

                rule.Compile(new[]
                {
                    model,
                    dualEnumerable,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Complete,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(3, executed);
            }

            [Fact]
            public void Should_ExecuteRepositoryValidator_ForSelectedType()
            {
                var model = new object();

                var member = new DualObjectsEnumerable();

                var executedValidatorForItemA = 0;
                var executedValidatorForItemB = 0;

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<ItemA>())
                    .Returns(be => be.Valid(m =>
                    {
                        Assert.Same(member.ItemsA.ElementAt(executedValidatorForItemA), m);

                        executedValidatorForItemA++;

                        return true;
                    }, "error"));

                validatorsRepositoryMock
                    .Setup(r => r.Get<ItemB>())
                    .Returns(be => be.Valid(m =>
                    {
                        Assert.Same(member.ItemsB.ElementAt(executedValidatorForItemB), m);

                        executedValidatorForItemB++;

                        return true;
                    }, "error"));

                var rule = new ValidCollectionRule<object, ItemB>(be => be
                    .ValidModel()
                );

                rule.Compile(new[]
                {
                    model,
                    member,
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Complete,
                    0,
                    new RulesOptionsStub()
                });

                Assert.Equal(3, executedValidatorForItemB);
                Assert.Equal(0, executedValidatorForItemA);
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
                    .Returns(be => be.For(m => m.Item2Instance, m => m.ValidCollection(i => i.ValidModel())));

                validatorsRepositoryMock
                    .Setup(r => r.Get<Item2>())
                    .Returns(be => be.For(m => m.Item3Instance, m => m.ValidCollection(i => i.ValidModel())));

                validatorsRepositoryMock
                    .Setup(r => r.Get<Item3>())
                    .Returns(be => be.Valid(m => true, "error"));

                var rule = new ValidCollectionRule<object, Item1>(be => be
                    .ValidModel()
                );

                Action compilation = () =>
                {
                    rule.Compile(new[]
                    {
                        new object(),
                        new[] {new Item1()},
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
                Validator<Item2> item2Validator = c => c.For(m => m.Item3Instance, m => m.ValidCollection(i => i.ValidModel(item3Validator)));
                Validator<Item1> item1Validator = c => c.For(m => m.Item2Instance, m => m.ValidCollection(i => i.ValidModel(item2Validator)));

                var rule = new ValidCollectionRule<object, Item1>(be => be
                    .ValidModel(item1Validator)
                );

                Action compilation = () =>
                {
                    rule.Compile(new[]
                    {
                        new object(),
                        new[] {new Item1()},
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
                    .Returns(be => be.For(m => m.Nested, m => m.ValidCollection(i => i.ValidModel())));

                var rule = new ValidCollectionRule<object, ItemLoop>(be => be.ValidModel());

                Assert.Throws<MaxDepthExceededException>(() =>
                {
                    rule.Compile(new[]
                    {
                        new object(),
                        new[] {new ItemLoop()},
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
                public IEnumerable<Item2> Item2Instance { get; } = new[] {new Item2()};
            }

            public class Item2
            {
                public IEnumerable<Item3> Item3Instance { get; } = new[] {new Item3()};
            }

            public class Item3
            {
            }

            public class ItemLoop
            {
                public ItemLoop()
                {
                    Nested = new[] {this};
                }

                public IEnumerable<ItemLoop> Nested { get; }
            }
        }

        [Fact]
        public void Should_ThrowException_When_NullCollect()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { new ValidCollectionRule<object, object>(null); });
        }

        [Fact]
        public void Should_ThrowException_When_WithName()
        {
            var model = new object();

            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            var rule = new ValidCollectionRule<object, object>(be => be
                .WithName("anything")
            );

            Assert.Throws<InvalidOperationException>(() =>
            {
                rule.Compile(new[]
                {
                    model,
                    new[] {new object()},
                    validatorsRepositoryMock.Object,
                    ValidationStrategy.Complete,
                    0,
                    new RulesOptionsStub()
                });
            });
        }
    }
}