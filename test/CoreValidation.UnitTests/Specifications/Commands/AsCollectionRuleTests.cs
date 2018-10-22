using System;
using System.Collections;
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
    public class AsCollectionRuleTests
    {
        public static IEnumerable<object[]> AllCollections_With_InvalidItems_Data()
        {
            var invalidItem = new object();

            yield return new[] {new[] {new object()}, invalidItem};
            yield return new[] {new[] {invalidItem}, invalidItem};
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

        public static bool AnyNonNullItem<T>(IEnumerable<T> member)
        {
            return (member != null) && member.Any(i => i != null);
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

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .Valid(v => v != invalidItem, "invalidItem2")
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.FailFast,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

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

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .Valid(v => v != invalidItem, "invalidItem2")
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

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

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .Valid(v => v != invalidItem, "invalidItem2")
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Force,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

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
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_AddErrors_When_Complete(object[] member, object invalidItem)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0,
                    out var errorsCollection);

                Assert.NotNull(errorsCollection);

                if (!member.Contains(invalidItem) && !member.Contains(null))
                {
                    Assert.False(getErrorsResult);
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.True(getErrorsResult);
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
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_AddErrors_When_Complete_And_SetSingleError(object[] member, object invalidItem)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .SetSingleError("invalidItem_overriden")
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0,
                    out var errorsCollection);

                Assert.NotNull(errorsCollection);

                if (!member.Contains(invalidItem) && !member.Contains(null))
                {
                    Assert.False(getErrorsResult);
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.True(getErrorsResult);
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
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_AddErrors_When_Complete_And_SetOptional(object[] member, object invalidItem)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .SetOptional()
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0,
                    out var errorsCollection);

                Assert.NotNull(errorsCollection);

                if (!member.Contains(invalidItem))
                {
                    Assert.False(getErrorsResult);
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.True(getErrorsResult);
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
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
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

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.FailFast,
                    0,
                    out var errorsCollection);

                Assert.NotNull(errorsCollection);

                if (!member.Contains(invalidItem) && !member.Contains(null))
                {
                    Assert.False(getErrorsResult);
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.True(getErrorsResult);
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
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_AddErrors_When_FailFast_And_SetOptional(object[] member, object invalidItem)
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

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .SetOptional()
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.FailFast,
                    0,
                    out var errorsCollection);

                Assert.NotNull(errorsCollection);

                if (!member.Contains(invalidItem))
                {
                    Assert.False(getErrorsResult);
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.True(getErrorsResult);
                    Assert.False(errorsCollection.IsEmpty);
                    Assert.Single(errorsCollection.Members);
                    Assert.Equal("invalidItem", errorsCollection.Members[firstInvalidIndex.ToString()].Errors.Single().Message);
                }
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_AddErrors_When_FailFast_And_SetSingleError(object[] member, object invalidItem)
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

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .SetSingleError("invalidItem_overriden")
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.FailFast,
                    0,
                    out var errorsCollection);

                Assert.NotNull(errorsCollection);

                if (!member.Contains(invalidItem) && !member.Contains(null))
                {
                    Assert.False(getErrorsResult);
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.True(getErrorsResult);
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
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_AddErrors_When_Force(object[] member, object invalidItem)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Force,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.False(errorsCollection.IsEmpty);

                Assert.Equal(1, errorsCollection.Members.Count);
                Assert.Equal(2, errorsCollection.Members["*"].Errors.Count);

                Assert.Equal("Required", errorsCollection.Members["*"].Errors.First().Message);
                Assert.Equal("invalidItem", errorsCollection.Members["*"].Errors.Last().Message);
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_AddErrors_When_Force_And_SetOptional(object[] member, object invalidItem)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .SetOptional()
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Force,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Equal(1, errorsCollection.Members.Count);
                Assert.Equal(1, errorsCollection.Members["*"].Errors.Count);

                Assert.Equal("invalidItem", errorsCollection.Members["*"].Errors.Last().Message);
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_AddErrors_When_Force_And_SetSingleError(object[] member, object invalidItem)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                    .SetSingleError("invalidItem_overriden")
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Force,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Equal(1, errorsCollection.Members.Count);
                Assert.Equal(2, errorsCollection.Members["*"].Errors.Count);

                Assert.Equal("Required", errorsCollection.Members["*"].Errors.ElementAt(0).Message);
                Assert.Equal("invalidItem_overriden", errorsCollection.Members["*"].Errors.ElementAt(1).Message);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddCustomRequiredError_When_SetRequiredError(ValidationStrategy validationStrategy)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => false, "invalidItem")
                    .SetRequired("Value is required (overriden)")
                );

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    new object[]
                    {
                        null
                    },
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

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

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var executionCounter = 0;

                var rule = new AsCollectionRule<object, int>(be => be
                    .Valid(m =>
                    {
                        Assert.Equal(member.ElementAt(executionCounter), m);

                        executionCounter++;

                        return true;
                    })
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0,
                    out _);

                Assert.Equal(member.Count, executionCounter);
            }

            [Theory]
            [MemberData(nameof(Strategies_AllCollections), new[] {ValidationStrategy.Complete}, MemberType = typeof(AsCollectionRuleTests))]
            public void Should_PassMemberReferenceToPredicate(ValidationStrategy validationStrategy, object[] member)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var nonNullIndexes = GetNotNullIndexes(member);

                var executionCounter = 0;

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(m =>
                    {
                        var nextNonNullIndex = nonNullIndexes.ElementAt(executionCounter);
                        Assert.Same(member.ElementAt(nextNonNullIndex), m);

                        executionCounter++;

                        return true;
                    })
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0,
                    out _);

                Assert.Equal(nonNullIndexes.Count, executionCounter);
            }

            [Theory]
            [MemberData(nameof(Strategies_AllCollections), new[] {ValidationStrategy.Complete}, MemberType = typeof(AsCollectionRuleTests))]
            public void Should_PassModelReferenceToPredicate(ValidationStrategy validationStrategy, object[] member)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var nonNullIndexes = GetNotNullIndexes(member);

                var executionCounter = 0;

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsRelative(m =>
                    {
                        Assert.Same(model, m);

                        executionCounter++;

                        return true;
                    })
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0,
                    out _);

                Assert.Equal(nonNullIndexes.Count, executionCounter);
            }

            [Theory]
            [MemberData(nameof(Strategies_AllCollections), new[] {ValidationStrategy.Complete}, MemberType = typeof(AsCollectionRuleTests))]
            public void Should_PassMemberReferenceToNestedPredicate_When_ValidatorDefined(ValidationStrategy validationStrategy, object[] member)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var nonNullIndexes = GetNotNullIndexes(member);

                var executionCounter = 0;

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsModel(n => n.Valid(m =>
                    {
                        var nextNonNullIndex = nonNullIndexes.ElementAt(executionCounter);
                        Assert.Same(member.ElementAt(nextNonNullIndex), m);

                        executionCounter++;

                        return true;
                    }))
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0,
                    out _);

                Assert.Equal(nonNullIndexes.Count, executionCounter);
            }

            [Theory]
            [MemberData(nameof(Strategies_AllCollections), new[] {ValidationStrategy.Complete}, MemberType = typeof(AsCollectionRuleTests))]
            public void Should_PassMemberReferenceToNestedPredicate_When_ValidatorFromRepository(ValidationStrategy validationStrategy, object[] member)
            {
                var model = new object();

                var nonNullIndexes = GetNotNullIndexes(member);

                var executionCounter = 0;

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(n => n.Valid(m =>
                    {
                        var nextNonNullIndex = nonNullIndexes.ElementAt(executionCounter);
                        Assert.Same(member.ElementAt(nextNonNullIndex), m);

                        executionCounter++;

                        return true;
                    }));

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsModel()
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0,
                    out _);

                Assert.Equal(nonNullIndexes.Count, executionCounter);
            }
        }

        public class ExecutingRepositorySpecification
        {
            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_ExecuteRepositorySpecification_When_Complete(object[] member)
            {
                var model = new object();

                var executedRepoSpecification = 0;

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoSpecification++;

                        return c;
                    });

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsModel()
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0,
                    out _);

                Assert.Equal(AnyNonNullItem(member) ? 1 : 0, executedRepoSpecification);
            }

            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_ExecuteRepositorySpecification_When_Force(object[] member)
            {
                var model = new object();

                var executedRepoSpecification = 0;

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoSpecification++;

                        return c;
                    });

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsModel()
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Force,
                    0,
                    out _);

                Assert.Equal(1, executedRepoSpecification);
            }

            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_ExecuteRepositorySpecification_When_ValidItemsWithNulls_FailFast(object[] member)
            {
                var model = new object();

                var executedRepoSpecification = 0;

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoSpecification++;

                        return c;
                    });

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsModel()
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.FailFast,
                    0,
                    out _);

                Assert.Equal(AnyNonNullItem(member) ? 1 : 0, executedRepoSpecification);
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_ExecuteRepositorySpecification_When_FailFast(object[] member, object invalidItem)
            {
                var model = new object();

                var executedRepoSpecification = 0;

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoSpecification++;

                        return c.Valid(m => m != invalidItem);
                    });

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsModel()
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.FailFast,
                    0,
                    out _);

                Assert.Equal(AnyNonNullItem(member) && (member.First() != null) ? 1 : 0, executedRepoSpecification);
            }
        }

        public class ExecutingDefinedSpecification
        {
            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_ExecuteDefinedSpecification_When_Complete(object[] member)
            {
                var model = new object();

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

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsModel(m =>
                    {
                        executedDefinedSpecification++;

                        return m;
                    })
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0,
                    out _);

                Assert.Equal(AnyNonNullItem(member) ? 1 : 0, executedDefinedSpecification);
                Assert.Equal(0, executedRepoSpecification);
            }

            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_ExecuteDefinedSpecification_When_Force(object[] member)
            {
                var model = new object();

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

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsModel(m =>
                    {
                        executedDefinedSpecification++;

                        return m;
                    })
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Force,
                    0,
                    out _);

                Assert.Equal(1, executedDefinedSpecification);
                Assert.Equal(0, executedRepoSpecification);
            }

            [Theory]
            [MemberData(nameof(AllCollections), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_ExecuteDefinedSpecification_When_ValidItemsWithNulls_FailFast(object[] member)
            {
                var model = new object();

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

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsModel(m =>
                    {
                        executedDefinedSpecification++;

                        return m;
                    })
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.FailFast,
                    0,
                    out _);

                Assert.Equal(AnyNonNullItem(member) && (member.First() != null) ? 1 : 0, executedDefinedSpecification);
                Assert.Equal(0, executedRepoSpecification);
            }

            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_ExecuteDefinedSpecification_When_FailFast(object[] member, object invalidItem)
            {
                var model = new object();

                var executedRepoSpecification = 0;
                var executedDefinedSpecification = 0;

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<object>())
                    .Returns(c =>
                    {
                        executedRepoSpecification++;

                        return c.Valid(m => m != invalidItem);
                    });

                var rule = new AsCollectionRule<object, object>(be => be
                    .AsModel(m =>
                    {
                        executedDefinedSpecification++;

                        return m.Valid(v => v != invalidItem);
                    })
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.FailFast,
                    0,
                    out _);

                Assert.Equal(AnyNonNullItem(member) && (member.First() != null) ? 1 : 0, executedDefinedSpecification);
                Assert.Equal(0, executedRepoSpecification);
            }
        }

        public class ExecutingDifferentSpecifications
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
            public void Should_ExecuteIntegerDefinedSpecification_When_MultipleEnumerators()
            {
                var model = new object();

                var dualEnumerable = new DualEnumerable();

                var executed = 0;

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, int>(be => be
                    .Valid(i =>
                    {
                        Assert.Equal(dualEnumerable.Integers.ElementAt(executed), i);

                        executed++;

                        return true;
                    }));

                rule.TryGetErrors(
                    model,
                    dualEnumerable,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0,
                    out _);

                Assert.Equal(3, executed);
            }

            [Fact]
            public void Should_ExecuteObjectDefinedSpecification_When_MultipleEnumerators()
            {
                var model = new object();

                var dualEnumerable = new DualEnumerable();

                var executed = 0;

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(i =>
                    {
                        Assert.Same(dualEnumerable.Objects.ElementAt(executed), i);

                        executed++;

                        return true;
                    }));

                rule.TryGetErrors(
                    model,
                    dualEnumerable,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0,
                    out _);

                Assert.Equal(3, executed);
            }

            [Fact]
            public void Should_ExecuteRepositorySpecification_ForSelectedType()
            {
                var model = new object();

                var member = new DualObjectsEnumerable();

                var executedSpecificationForItemA = 0;
                var executedSpecificationForItemB = 0;

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(r => r.Get<ItemA>())
                    .Returns(be => be.Valid(m =>
                    {
                        Assert.Same(member.ItemsA.ElementAt(executedSpecificationForItemA), m);

                        executedSpecificationForItemA++;

                        return true;
                    }));

                specificationsRepositoryMock
                    .Setup(r => r.Get<ItemB>())
                    .Returns(be => be.Valid(m =>
                    {
                        Assert.Same(member.ItemsB.ElementAt(executedSpecificationForItemB), m);

                        executedSpecificationForItemB++;

                        return true;
                    }));

                var rule = new AsCollectionRule<object, ItemB>(be => be
                    .AsModel()
                );

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0,
                    out _);

                Assert.Equal(3, executedSpecificationForItemB);
                Assert.Equal(0, executedSpecificationForItemA);
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
                    .Returns(be => be.Member(m => m.Item2Instance, m => m.AsCollection(i => i.AsModel())));

                specificationsRepositoryMock
                    .Setup(r => r.Get<Item2>())
                    .Returns(be => be.Member(m => m.Item3Instance, m => m.AsCollection(i => i.AsModel())));

                specificationsRepositoryMock
                    .Setup(r => r.Get<Item3>())
                    .Returns(be => be.Valid(m => true));

                var rule = new AsCollectionRule<object, Item1>(be => be
                    .AsModel()
                );

                Action compilation = () =>
                {
                    rule.TryGetErrors(
                        new object(),
                        new[] {new Item1()},
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
                Specification<Item2> item2Specification = c => c.Member(m => m.Item3Instance, m => m.AsCollection(i => i.AsModel(item3Specification)));
                Specification<Item1> item1Specification = c => c.Member(m => m.Item2Instance, m => m.AsCollection(i => i.AsModel(item2Specification)));

                var rule = new AsCollectionRule<object, Item1>(be => be
                    .AsModel(item1Specification)
                );

                Action compilation = () =>
                {
                    rule.TryGetErrors(
                        new object(),
                        new[] {new Item1()},
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
                    .Returns(be => be.Member(m => m.Nested, m => m.AsCollection(i => i.AsModel())));

                var rule = new AsCollectionRule<object, ItemLoop>(be => be.AsModel());

                Assert.Throws<MaxDepthExceededException>(() =>
                {
                    rule.TryGetErrors(
                        new object(),
                        new[] {new ItemLoop()},
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

        public class RuleSingleError
        {
            [Theory]
            [MemberData(nameof(AllCollections_With_InvalidItems_Data), MemberType = typeof(AsCollectionRuleTests))]
            public void Should_ReturnRuleSingleError(object[] member, object invalidItem)
            {
                var model = new object();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var rule = new AsCollectionRule<object, object>(be => be
                    .Valid(v => v != invalidItem, "invalidItem")
                );

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0,
                    out var errorsCollection);

                Assert.NotNull(errorsCollection);

                if (!member.Contains(invalidItem) && !member.Contains(null))
                {
                    Assert.False(getErrorsResult);
                    Assert.True(errorsCollection.IsEmpty);
                }
                else
                {
                    Assert.True(getErrorsResult);
                    Assert.False(errorsCollection.IsEmpty);
                    Assert.Empty(errorsCollection.Members);

                    Assert.Equal("ruleSingleError", errorsCollection.Errors.Single().Message);
                }
            }
        }

        [Fact]
        public void Should_ThrowException_When_NullCollect()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { new AsCollectionRule<object, object>(null); });
        }
    }
}