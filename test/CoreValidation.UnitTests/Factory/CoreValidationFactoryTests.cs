using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors.Args;
using CoreValidation.Exceptions;
using CoreValidation.Factory;
using CoreValidation.Factory.Specifications;
using CoreValidation.Factory.Translations;
using CoreValidation.Options;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

// ReSharper disable HeapView.ObjectAllocation.Evident

namespace CoreValidation.UnitTests.Factory
{
    public class CoreValidationFactoryTests
    {
        public class User
        {
        }

        public class Address
        {
        }

        public class Details
        {
        }

        public class Translations
        {
            public static IEnumerable<object[]> Create_Should_ThrowException_When_SetNonExistingTranslationName_Data()
            {
                yield return new object[]
                {
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o.SetTranslationName("non_existing"))
                };

                yield return new object[]
                {
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                        .SetTranslationName("c"))
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_ThrowException_When_SetNonExistingTranslationName_Data))]
            public void Create_Should_ThrowException_When_SetNonExistingTranslationName(Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.Throws<TranslationNotFoundException>(() => new ValidationContextFactory().Create(options));
            }

            public static IEnumerable<object[]> Create_Should_SetTranslationName_Data()
            {
                yield return new object[]
                {
                    "t2",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                        .AddTranslation("t3", new Dictionary<string, string> {{"c", "C"}})
                        .SetTranslationName("t2")
                    )
                };

                yield return new object[]
                {
                    "t1",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                        .SetTranslationName("t3")
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                        .SetTranslationName("t2")
                        .AddTranslation("t3", new Dictionary<string, string> {{"c", "C"}})
                        .SetTranslationName("t1")
                    )
                };

                yield return new object[]
                {
                    "t3",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetTranslationName("t1")
                        .SetTranslationName("t2")
                        .SetTranslationName("t3")
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                        .AddTranslation("t3", new Dictionary<string, string> {{"c", "C"}})
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_SetTranslationName_Data))]
            public static void Create_Should_SetTranslationName(string translationName, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);
                Assert.NotEmpty(validationContext.Translations);

                Assert.Equal(translationName, validationContext.ValidationOptions.TranslationName);
            }

            public static IEnumerable<object[]> Create_Should_AddTranslation_AndSetAsDefault_Data()
            {
                yield return new object[]
                {
                    "t2",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}}, true)
                        .AddTranslation("t3", new Dictionary<string, string> {{"c", "C"}})
                    )
                };

                yield return new object[]
                {
                    "t3",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}}, true)
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}}, true)
                        .AddTranslation("t3", new Dictionary<string, string> {{"c", "C"}}, true)
                    )
                };

                yield return new object[]
                {
                    "t1",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}}, true)
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                        .AddTranslation("t3", new Dictionary<string, string> {{"c", "C"}})
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_AddTranslation_AndSetAsDefault_Data))]
            public static void Create_Should_AddTranslation_AndSetAsDefault(string translationName, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);
                Assert.NotEmpty(validationContext.Translations);

                Assert.Equal(translationName, validationContext.ValidationOptions.TranslationName);
            }

            public static IEnumerable<object[]> Create_Should_SetTranslationDisabled_Data()
            {
                yield return new object[]
                {
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}}, true)
                        .SetTranslationDisabled()
                    )
                };

                yield return new object[]
                {
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                        .SetTranslationName("t1")
                        .SetTranslationDisabled()
                    )
                };

                yield return new object[]
                {
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                        .SetTranslationDisabled()
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_SetTranslationDisabled_Data))]
            public static void Create_Should_SetTranslationDisabled(Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);
                Assert.NotEmpty(validationContext.Translations);

                Assert.Null(validationContext.ValidationOptions.TranslationName);
            }

            public static IEnumerable<object[]> Create_Should_AddTranslation_Data()
            {
                yield return new object[]
                {
                    "Separated",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                        .AddTranslation("t1", new Dictionary<string, string> {{"a1", "A1"}})
                        .AddTranslation("t1", new Dictionary<string, string> {{"a2", "A2"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}}))
                };

                yield return new object[]
                {
                    "TranslationsGrouped",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}, {"a1", "A1"}, {"a2", "A2"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}}))
                };

                yield return new object[]
                {
                    "TranslationsGrouped_And_Overriden",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "Ax"}, {"a1", "A1x"}, {"a2", "A2x"}})
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}, {"a1", "A1"}, {"a2", "A2"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "Bx"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                    )
                };

                yield return new object[]
                {
                    "TranslationsOverridenByPackages",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "Ax"}, {"a1", "A1x"}, {"a2", "A2x"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "Bx"}})
                        .AddTranslations(new TranslationsPackage {{"t1", new Dictionary<string, string> {{"a", "A"}, {"a1", "A1"}, {"a2", "A2"}}}})
                        .AddTranslations(new TranslationsPackage {{"t2", new Dictionary<string, string> {{"b", "B"}}}})
                    )
                };

                yield return new object[]
                {
                    "TranslationsOverridenByPackage",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "Ax"}, {"a1", "A1x"}, {"a2", "A2x"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "Bx"}})
                        .AddTranslations(new TranslationsPackage
                        {
                            {"t1", new Dictionary<string, string> {{"a", "A"}, {"a1", "A1"}, {"a2", "A2"}}},
                            {"t2", new Dictionary<string, string> {{"b", "B"}}}
                        })
                    )
                };

                yield return new object[]
                {
                    "SinglePackage",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslations(new TranslationsPackage
                        {
                            {"t1", new Dictionary<string, string> {{"a", "A"}, {"a1", "A1"}, {"a2", "A2"}}},
                            {"t2", new Dictionary<string, string> {{"b", "B"}}}
                        })
                    )
                };

                yield return new object[]
                {
                    "MultiplePackages",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslations(new TranslationsPackage
                        {
                            {"t1", new Dictionary<string, string> {{"a", "Ax"}, {"a1", "A1x"}, {"a2", "A2x"}}},
                            {"t2", new Dictionary<string, string> {{"b", "Bx"}}}
                        })
                        .AddTranslations(new TranslationsPackage
                        {
                            {"t1", new Dictionary<string, string> {{"a", "A"}, {"a1", "A1"}, {"a2", "A2"}}},
                            {"t2", new Dictionary<string, string> {{"b", "B"}}}
                        })
                    )
                };

                yield return new object[]
                {
                    "PackagesOverridenByTranslations",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslations(new TranslationsPackage
                        {
                            {"t1", new Dictionary<string, string> {{"a", "Ax"}, {"a1", "A1x"}, {"a2", "A2x"}}},
                            {"t2", new Dictionary<string, string> {{"b", "Bx"}}}
                        })
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}, {"a1", "A1"}, {"a2", "A2"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                    )
                };

                yield return new object[]
                {
                    "MixedAndMultipleOverrides",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddTranslation("t1", new Dictionary<string, string> {{"a", "Ax"}})
                        .AddTranslation("t2", new Dictionary<string, string> {{"b", "Bx"}})
                        .AddTranslations(new TranslationsPackage
                        {
                            {"t1", new Dictionary<string, string> {{"a", "A"}, {"a1", "A1x"}, {"a2", "A2x"}}},
                            {"t2", new Dictionary<string, string> {{"b", "B"}}}
                        })
                        .AddTranslation("t1", new Dictionary<string, string> {{"a1", "A1"}, {"a2", "A2xx"}})
                        .AddTranslations(new TranslationsPackage
                        {
                            {"t1", new Dictionary<string, string> {{"a2", "A2"}}}
                        })
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_AddTranslation_Data))]
            public static void Create_Should_AddTranslation(string debugName, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.NotNull(debugName);

                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);
                Assert.NotEmpty(validationContext.Translations);

                Assert.Null(validationContext.ValidationOptions.TranslationName);

                Assert.Equal(2, validationContext.Translations.Keys.Count());

                Assert.Equal(3, validationContext.Translations["t1"].Count);
                Assert.Equal("A", validationContext.Translations["t1"]["a"]);
                Assert.Equal("A1", validationContext.Translations["t1"]["a1"]);
                Assert.Equal("A2", validationContext.Translations["t1"]["a2"]);

                Assert.Equal(1, validationContext.Translations["t2"].Count);
                Assert.Equal("B", validationContext.Translations["t2"]["b"]);
            }

            [Fact]
            public void Create_Should_ThrowException_When_NullDictionary()
            {
                Assert.Throws<ArgumentNullException>(() => new ValidationContextFactory().Create(o => o
                    .AddTranslation("test", null)
                ));
            }

            [Fact]
            public void Create_Should_ThrowException_When_NullPackage()
            {
                Assert.Throws<ArgumentNullException>(() => new ValidationContextFactory().Create(o => o
                    .AddTranslations(null)
                ));
            }

            [Fact]
            public void Create_Should_ThrowException_When_NullThis()
            {
                Assert.Throws<ArgumentNullException>(() => new ValidationContextFactory().Create(o => (null as IValidationContextOptions)
                    .AddTranslation(null, new Dictionary<string, string> {{"a", "A"}})
                ));
            }

            [Fact]
            public void Create_Should_ThrowException_When_NullTranslationName()
            {
                Assert.Throws<ArgumentNullException>(() => new ValidationContextFactory().Create(o => o
                    .AddTranslation(null, new Dictionary<string, string> {{"a", "A"}})
                ));
            }
        }

        public class Specifications
        {
            private class InvalidEmptySpecificationHolder : ISpecificationHolder
            {
            }

            private class InvalidNullSpecificationHolder : ISpecificationHolder<User>
            {
                public Specification<User> Specification { get; } = null;
            }

            private class UserSpecificationHolder : ISpecificationHolder<User>
            {
                public UserSpecificationHolder(Specification<User> specification)
                {
                    Specification = specification;
                }

                public Specification<User> Specification { get; }
            }

            private class AddressSpecificationHolder : ISpecificationHolder<Address>
            {
                public AddressSpecificationHolder(Specification<Address> specification)
                {
                    Specification = specification;
                }

                public Specification<Address> Specification { get; }
            }

            private class DetailsSpecificationHolder : ISpecificationHolder<Details>
            {
                public DetailsSpecificationHolder(Specification<Details> specification)
                {
                    Specification = specification;
                }

                public Specification<Details> Specification { get; }
            }

            private class AllInOneSpecificationHolder : ISpecificationHolder<User>, ISpecificationHolder<Address>, ISpecificationHolder<Details>
            {
                private readonly Specification<Address> _addressSpecification;

                private readonly Specification<Details> _detailsSpecification;

                private readonly Specification<User> _userSpecification;

                public AllInOneSpecificationHolder(Specification<User> userSpecification, Specification<Address> addressSpecification, Specification<Details> detailsSpecification)
                {
                    _userSpecification = userSpecification;
                    _addressSpecification = addressSpecification;
                    _detailsSpecification = detailsSpecification;
                }

                Specification<Address> ISpecificationHolder<Address>.Specification => _addressSpecification;

                Specification<Details> ISpecificationHolder<Details>.Specification => _detailsSpecification;

                Specification<User> ISpecificationHolder<User>.Specification => _userSpecification;
            }

            public static IEnumerable<object[]> Create_Should_AddSpecification_Data()
            {
                var userSpecification = new Specification<User>(c => c);
                var addressSpecification = new Specification<Address>(c => c);
                var detailsSpecification = new Specification<Details>(c => c);

                yield return new object[]
                {
                    "AddSpecification", userSpecification, addressSpecification, detailsSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification(userSpecification)
                        .AddSpecification(addressSpecification)
                        .AddSpecification(detailsSpecification)
                    )
                };

                yield return new object[]
                {
                    "AddSpecification_WithOverrides", userSpecification, addressSpecification, detailsSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification<User>(c => c)
                        .AddSpecification<Address>(c => c)
                        .AddSpecification<Details>(c => c)
                        .AddSpecification(userSpecification)
                        .AddSpecification(addressSpecification)
                        .AddSpecification(detailsSpecification)
                    )
                };

                yield return new object[]
                {
                    "AddSpecification_WithOverrides_FromHolder", userSpecification, addressSpecification, detailsSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification<User>(c => c)
                        .AddSpecification<Address>(c => c)
                        .AddSpecification<Details>(c => c)
                        .AddSpecificationsFromHolder(new UserSpecificationHolder(userSpecification))
                        .AddSpecificationsFromHolder(new AddressSpecificationHolder(addressSpecification))
                        .AddSpecificationsFromHolder(new DetailsSpecificationHolder(detailsSpecification))
                    )
                };

                yield return new object[]
                {
                    "AddSpecificationsFromHolder", userSpecification, addressSpecification, detailsSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecificationsFromHolder(new UserSpecificationHolder(userSpecification))
                        .AddSpecificationsFromHolder(new AddressSpecificationHolder(addressSpecification))
                        .AddSpecificationsFromHolder(new DetailsSpecificationHolder(detailsSpecification))
                    )
                };

                yield return new object[]
                {
                    "AddSpecificationsFromHolder_AllInOne", userSpecification, addressSpecification, detailsSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecificationsFromHolder(new AllInOneSpecificationHolder(userSpecification, addressSpecification, detailsSpecification))
                    )
                };

                yield return new object[]
                {
                    "AddSpecification_WithOverrides_FromHolder_AllInOne", userSpecification, addressSpecification, detailsSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification<User>(c => c)
                        .AddSpecification<Address>(c => c)
                        .AddSpecification<Details>(c => c)
                        .AddSpecificationsFromHolder(new AllInOneSpecificationHolder(userSpecification, addressSpecification, detailsSpecification))
                    )
                };

                yield return new object[]
                {
                    "Mixed", userSpecification, addressSpecification, detailsSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification<User>(c => c)
                        .AddSpecificationsFromHolder(new AddressSpecificationHolder(c => c))
                        .AddSpecificationsFromHolder(new AllInOneSpecificationHolder(c => c, addressSpecification, c => c))
                        .AddSpecificationsFromHolder(new DetailsSpecificationHolder(detailsSpecification))
                        .AddSpecification(userSpecification)
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_AddSpecification_Data))]
            public static void Create_Should_AddSpecification(string debugName, Specification<User> userSpecification, Specification<Address> addressSpecification, Specification<Details> detailsSpecification, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.NotNull(debugName);

                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);
                Assert.NotEmpty(validationContext.Types);

                Assert.Equal(3, validationContext.Types.Count);
                ValidationContextTestsHelper.AssertSpecification(validationContext, userSpecification);
                ValidationContextTestsHelper.AssertSpecification(validationContext, addressSpecification);
                ValidationContextTestsHelper.AssertSpecification(validationContext, detailsSpecification);
            }

            [Fact]
            public void Create_Should_ThrowException_When_AddHolderWithNoSpecifications()
            {
                Assert.Throws<InvalidOperationException>(() => new ValidationContextFactory().Create(o => o.AddSpecificationsFromHolder(new InvalidEmptySpecificationHolder())));
            }

            [Fact]
            public void Create_Should_ThrowException_When_AddHolderWithNullSpecification()
            {
                Assert.Throws<InvalidOperationException>(() => new ValidationContextFactory().Create(o => o.AddSpecificationsFromHolder(new InvalidNullSpecificationHolder())));
            }

            [Fact]
            public void Create_Should_ThrowException_When_AddNullSpecification()
            {
                Assert.Throws<ArgumentNullException>(() => new ValidationContextFactory().Create(o => o.AddSpecification<User>(null)));
            }

            [Fact]
            public void Create_Should_ThrowException_When_AddNullSpecificationHolder()
            {
                Assert.Throws<ArgumentNullException>(() => new ValidationContextFactory().Create(o => o.AddSpecificationsFromHolder(null)));
            }
        }

        public class NullRootStrategies
        {
            public static IEnumerable<object[]> Create_Should_SetNullRootStrategy_Data()
            {
                yield return new object[]
                {
                    "SingleSet",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetNullRootStrategy(NullRootStrategy.NoErrors)
                    )
                };

                yield return new object[]
                {
                    "Override",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetNullRootStrategy(NullRootStrategy.ArgumentNullException)
                        .SetNullRootStrategy(NullRootStrategy.NoErrors)
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_SetNullRootStrategy_Data))]
            public static void Create_Should_SetNullRootStrategy(string debugName, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.NotNull(debugName);

                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);

                Assert.Equal(NullRootStrategy.NoErrors, validationContext.ValidationOptions.NullRootStrategy);
            }
        }

        public class ValidationStrategies
        {
            public static IEnumerable<object[]> Create_Should_SetValidationStrategy_Data()
            {
                yield return new object[]
                {
                    "SingleSet",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetValidationStrategy(ValidationStrategy.Force)
                    )
                };

                yield return new object[]
                {
                    "Override",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetValidationStrategy(ValidationStrategy.Complete)
                        .SetValidationStrategy(ValidationStrategy.Force)
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_SetValidationStrategy_Data))]
            public static void Create_Should_SetValidationStrategy(string debugName, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.NotNull(debugName);

                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);

                Assert.Equal(ValidationStrategy.Force, validationContext.ValidationOptions.ValidationStrategy);
            }
        }

        public class SetCollectionForceKey
        {
            public static IEnumerable<object[]> Create_Should_SetCollectionForceKey_Data()
            {
                yield return new object[]
                {
                    "SingleSet",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetCollectionForceKey("[*]")
                    )
                };

                yield return new object[]
                {
                    "Override",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetCollectionForceKey("__")
                        .SetCollectionForceKey("[*]")
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_SetCollectionForceKey_Data))]
            public static void Create_Should_SetCollectionForceKey(string debugName, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.NotNull(debugName);

                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);

                Assert.Equal("[*]", validationContext.ValidationOptions.CollectionForceKey);
            }

            [Fact]
            public static void Create_Should_ThrowException_When_NullCollectionForceKey()
            {
                Assert.Throws<ArgumentNullException>(() => { new ValidationContextFactory().Create(options => options.SetCollectionForceKey(null)); });
            }
        }

        public class SetMaxDepth
        {
            public static IEnumerable<object[]> Create_Should_SetMaxDepth_Data()
            {
                yield return new object[]
                {
                    "SingleSet",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetMaxDepth(13)
                    )
                };

                yield return new object[]
                {
                    "Override",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetMaxDepth(100)
                        .SetMaxDepth(13)
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_SetMaxDepth_Data))]
            public static void Create_Should_SetMaxDepth(string debugName, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.NotNull(debugName);

                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);

                Assert.Equal(13, validationContext.ValidationOptions.MaxDepth);
            }

            [Fact]
            public static void Create_Should_ThrowException_When_NegativeMaxDepth()
            {
                Assert.Throws<InvalidOperationException>(() => { new ValidationContextFactory().Create(options => options.SetMaxDepth(-1)); });
            }
        }

        public class SetRequiredError
        {
            public static IEnumerable<object[]> Create_Should_SetRequiredError_Data()
            {
                yield return new object[]
                {
                    "SingleSet",
                    "This is REQUIRED!",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetRequiredError("This is REQUIRED!")
                    )
                };

                yield return new object[]
                {
                    "Override",
                    "This is REQUIRED!",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetRequiredError("XX")
                        .SetRequiredError("This is REQUIRED!")
                    )
                };

                yield return new object[]
                {
                    "WithArgs",
                    "This {arg1} REQUIRED{arg2}",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetRequiredError("This {arg1} REQUIRED{arg2}", new IMessageArg[] {new MessageArg("arg1", "is"), new MessageArg("arg2", "!")})
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_SetRequiredError_Data))]
            public static void Create_Should_SetRequiredError(string debugName, string message, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.NotNull(debugName);

                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);

                Assert.Equal(message, validationContext.ValidationOptions.RequiredError.Message);
                Assert.Equal("This is REQUIRED!", validationContext.ValidationOptions.RequiredError.FormattedMessage);
            }

            [Fact]
            public static void Create_Should_ThrowException_When_NullInRequiredErrorMessageArgs()
            {
                Assert.Throws<ArgumentNullException>(() => { new ValidationContextFactory().Create(options => options.SetRequiredError("test", new IMessageArg[] {null})); });
            }

            [Fact]
            public static void Create_Should_ThrowException_When_NullRequiredErrorMessage()
            {
                Assert.Throws<ArgumentNullException>(() => { new ValidationContextFactory().Create(options => options.SetRequiredError(null)); });
            }
        }

        public class SetDefaultError
        {
            public static IEnumerable<object[]> Create_Should_SetDefaultError_Data()
            {
                yield return new object[]
                {
                    "SingleSet",
                    "This is DEFAULT!",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetRequiredError("This is DEFAULT!")
                    )
                };

                yield return new object[]
                {
                    "Override",
                    "This is DEFAULT!",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetRequiredError("XX")
                        .SetRequiredError("This is DEFAULT!")
                    )
                };

                yield return new object[]
                {
                    "WithArgs",
                    "This {arg1} DEFAULT{arg2}",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetRequiredError("This {arg1} DEFAULT{arg2}", new IMessageArg[] {new MessageArg("arg1", "is"), new MessageArg("arg2", "!")})
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_SetDefaultError_Data))]
            public static void Create_Should_SetDefaultError(string debugName, string message, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.NotNull(debugName);

                var validationContext = new ValidationContextFactory().Create(options);

                Assert.NotNull(validationContext);

                Assert.Equal(message, validationContext.ValidationOptions.RequiredError.Message);
                Assert.Equal("This is DEFAULT!", validationContext.ValidationOptions.RequiredError.FormattedMessage);
            }

            [Fact]
            public static void Create_Should_ThrowException_When_NullDefaultErrorMessage()
            {
                Assert.Throws<ArgumentNullException>(() => { new ValidationContextFactory().Create(options => options.SetDefaultError(null)); });
            }

            [Fact]
            public static void Create_Should_ThrowException_When_NullInDefaultErrorMessageArgs()
            {
                Assert.Throws<ArgumentNullException>(() => { new ValidationContextFactory().Create(options => options.SetDefaultError("test", new IMessageArg[] {null})); });
            }
        }

        [Fact]
        public void Create_Should_ProcessOptions()
        {
            var userSpecification = new Specification<User>(m => m);

            var validationContext = new ValidationContextFactory().Create(o => o
                .AddSpecification(userSpecification)
                .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                .SetNullRootStrategy(NullRootStrategy.NoErrors)
                .SetTranslationName("t1")
                .SetValidationStrategy(ValidationStrategy.FailFast)
                .SetCollectionForceKey("[]")
                .SetMaxDepth(15)
                .SetRequiredError("This is required{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                .SetDefaultError("This is default{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
            );

            Assert.NotNull(validationContext);

            Assert.Equal(1, validationContext.Types.Count);
            ValidationContextTestsHelper.AssertSpecification(validationContext, userSpecification);

            Assert.Equal("t1", validationContext.Translations.Keys.Single());
            Assert.Single(validationContext.Translations["t1"]);
            Assert.Equal("A", validationContext.Translations["t1"]["a"]);

            Assert.Equal(NullRootStrategy.NoErrors, validationContext.ValidationOptions.NullRootStrategy);
            Assert.Equal("t1", validationContext.ValidationOptions.TranslationName);
            Assert.Equal(ValidationStrategy.FailFast, validationContext.ValidationOptions.ValidationStrategy);
            Assert.Equal("[]", validationContext.ValidationOptions.CollectionForceKey);
            Assert.Equal(15, validationContext.ValidationOptions.MaxDepth);
            Assert.Equal("This is required{arg}", validationContext.ValidationOptions.RequiredError.Message);
            Assert.Equal("This is required!", validationContext.ValidationOptions.RequiredError.FormattedMessage);
            Assert.Equal("This is default{arg}", validationContext.ValidationOptions.DefaultError.Message);
            Assert.Equal("This is default!", validationContext.ValidationOptions.DefaultError.FormattedMessage);
        }

        [Fact]
        public void Create_Should_ProcessOptions_When_Default()
        {
            var validationContext = new ValidationContextFactory().Create();

            Assert.NotNull(validationContext);
            Assert.Empty(validationContext.Translations);
            Assert.Empty(validationContext.Types);

            Assert.Equal(NullRootStrategy.RequiredError, validationContext.ValidationOptions.NullRootStrategy);
            Assert.Null(validationContext.ValidationOptions.TranslationName);
            Assert.Equal(ValidationStrategy.Complete, validationContext.ValidationOptions.ValidationStrategy);
            Assert.Equal("*", validationContext.ValidationOptions.CollectionForceKey);
            Assert.Equal(10, validationContext.ValidationOptions.MaxDepth);
            Assert.Equal("Required", validationContext.ValidationOptions.RequiredError.Message);
            Assert.Equal("Required", validationContext.ValidationOptions.RequiredError.FormattedMessage);
            Assert.Equal("Invalid", validationContext.ValidationOptions.DefaultError.Message);
            Assert.Equal("Invalid", validationContext.ValidationOptions.DefaultError.FormattedMessage);
        }

        [Fact]
        public void Create_Should_ProcessOptions_WithMoreAssignments()
        {
            var userSpecification = new Specification<User>(m => m);
            var addressSpecification = new Specification<Address>(m => m);

            var validationContext = new ValidationContextFactory().Create(o => o
                .AddSpecification(userSpecification)
                .AddSpecification(addressSpecification)
                .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                .AddTranslation("t1", new Dictionary<string, string> {{"a1", "A1"}})
                .AddTranslation("t1", new Dictionary<string, string> {{"a2", "A2"}, {"a", "AA"}})
                .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                .SetNullRootStrategy(NullRootStrategy.NoErrors)
                .SetNullRootStrategy(NullRootStrategy.ArgumentNullException)
                .SetTranslationName("t1")
                .SetTranslationName("t2")
                .SetValidationStrategy(ValidationStrategy.FailFast)
                .SetValidationStrategy(ValidationStrategy.Force)
                .SetCollectionForceKey("[]")
                .SetCollectionForceKey("[*]")
                .SetMaxDepth(15)
                .SetMaxDepth(20)
                .SetRequiredError("This is required{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                .SetRequiredError("This is required{arg}. True story.", new IMessageArg[] {new MessageArg("arg", "!")})
                .SetDefaultError("This is default{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                .SetDefaultError("This is default{arg}. Legit.", new IMessageArg[] {new MessageArg("arg", "!")})
            );

            Assert.NotNull(validationContext);
            Assert.Equal(2, validationContext.Types.Count);

            ValidationContextTestsHelper.AssertSpecification(validationContext, userSpecification);
            ValidationContextTestsHelper.AssertSpecification(validationContext, userSpecification);

            Assert.Equal(2, validationContext.Translations.Keys.Count());

            Assert.Equal(3, validationContext.Translations["t1"].Count);
            Assert.Equal("AA", validationContext.Translations["t1"]["a"]);
            Assert.Equal("A1", validationContext.Translations["t1"]["a1"]);
            Assert.Equal("A2", validationContext.Translations["t1"]["a2"]);

            Assert.Equal(1, validationContext.Translations["t2"].Count);
            Assert.Equal("B", validationContext.Translations["t2"]["b"]);

            Assert.Equal(NullRootStrategy.ArgumentNullException, validationContext.ValidationOptions.NullRootStrategy);
            Assert.Equal("t2", validationContext.ValidationOptions.TranslationName);
            Assert.Equal(ValidationStrategy.Force, validationContext.ValidationOptions.ValidationStrategy);
            Assert.Equal("[*]", validationContext.ValidationOptions.CollectionForceKey);
            Assert.Equal(20, validationContext.ValidationOptions.MaxDepth);
            Assert.Equal("This is required{arg}. True story.", validationContext.ValidationOptions.RequiredError.Message);
            Assert.Equal("This is required!. True story.", validationContext.ValidationOptions.RequiredError.FormattedMessage);
            Assert.Equal("This is default{arg}. Legit.", validationContext.ValidationOptions.DefaultError.Message);
            Assert.Equal("This is default!. Legit.", validationContext.ValidationOptions.DefaultError.FormattedMessage);
        }

        [Fact]
        public void Create_Should_ThrowException_When_ReturningNewInstance()
        {
            var exception = Assert.Throws<InvalidProcessedReferenceException>(() => new ValidationContextFactory().Create(o => new ValidationContextOptions()));

            Assert.Equal(typeof(IValidationContextOptions), exception.Type);
        }
    }
}