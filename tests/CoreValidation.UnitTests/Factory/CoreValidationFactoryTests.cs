using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Exceptions;
using CoreValidation.Factory;
using CoreValidation.Factory.Translations;
using CoreValidation.Factory.Validators;
using CoreValidation.Options;
using CoreValidation.Translations;
using CoreValidation.Validators;
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
                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);
                Assert.NotEmpty(coreValidator.Translations);

                Assert.Equal(translationName, coreValidator.ValidationOptions.TranslationName);
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
                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);
                Assert.NotEmpty(coreValidator.Translations);

                Assert.Equal(translationName, coreValidator.ValidationOptions.TranslationName);
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
                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);
                Assert.NotEmpty(coreValidator.Translations);

                Assert.Null(coreValidator.ValidationOptions.TranslationName);
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

                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);
                Assert.NotEmpty(coreValidator.Translations);

                Assert.Null(coreValidator.ValidationOptions.TranslationName);

                Assert.Equal(2, coreValidator.Translations.Keys.Count());

                Assert.Equal(3, coreValidator.Translations["t1"].Count);
                Assert.Equal("A", coreValidator.Translations["t1"]["a"]);
                Assert.Equal("A1", coreValidator.Translations["t1"]["a1"]);
                Assert.Equal("A2", coreValidator.Translations["t1"]["a2"]);

                Assert.Equal(1, coreValidator.Translations["t2"].Count);
                Assert.Equal("B", coreValidator.Translations["t2"]["b"]);
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
            public void Create_Should_ThrowException_When_NullTranslationName()
            {
                Assert.Throws<ArgumentNullException>(() => new ValidationContextFactory().Create(o => o
                    .AddTranslation(null, new Dictionary<string, string> {{"a", "A"}})
                ));
            }
        }

        public class Validators
        {
            private class UserValidatorHolder : IValidatorHolder<User>
            {
                public UserValidatorHolder(Validator<User> validator)
                {
                    Validator = validator;
                }

                public Validator<User> Validator { get; }
            }

            private class AddressValidatorHolder : IValidatorHolder<Address>
            {
                public AddressValidatorHolder(Validator<Address> validator)
                {
                    Validator = validator;
                }

                public Validator<Address> Validator { get; }
            }

            private class DetailsValidatorHolder : IValidatorHolder<Details>
            {
                public DetailsValidatorHolder(Validator<Details> validator)
                {
                    Validator = validator;
                }

                public Validator<Details> Validator { get; }
            }

            private class AllInOneValidatorHolder : IValidatorHolder<User>, IValidatorHolder<Address>, IValidatorHolder<Details>
            {
                private readonly Validator<Address> _addressValidator;

                private readonly Validator<Details> _detailsValidator;

                private readonly Validator<User> _userValidator;

                public AllInOneValidatorHolder(Validator<User> userValidator, Validator<Address> addressValidator, Validator<Details> detailsValidator)
                {
                    _userValidator = userValidator;
                    _addressValidator = addressValidator;
                    _detailsValidator = detailsValidator;
                }

                Validator<Address> IValidatorHolder<Address>.Validator => _addressValidator;

                Validator<Details> IValidatorHolder<Details>.Validator => _detailsValidator;

                Validator<User> IValidatorHolder<User>.Validator => _userValidator;
            }

            public static IEnumerable<object[]> Create_Should_AddValidator_Data()
            {
                var userValidator = new Validator<User>(c => c);
                var addressValidator = new Validator<Address>(c => c);
                var detailsValidator = new Validator<Details>(c => c);

                yield return new object[]
                {
                    "AddValidator", userValidator, addressValidator, detailsValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator(userValidator)
                        .AddValidator(addressValidator)
                        .AddValidator(detailsValidator)
                    )
                };

                yield return new object[]
                {
                    "AddValidator_WithOverrides", userValidator, addressValidator, detailsValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator<User>(c => c)
                        .AddValidator<Address>(c => c)
                        .AddValidator<Details>(c => c)
                        .AddValidator(userValidator)
                        .AddValidator(addressValidator)
                        .AddValidator(detailsValidator)
                    )
                };

                yield return new object[]
                {
                    "AddValidator_WithOverrides_FromHolder", userValidator, addressValidator, detailsValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator<User>(c => c)
                        .AddValidator<Address>(c => c)
                        .AddValidator<Details>(c => c)
                        .AddValidatorsFromHolder(new UserValidatorHolder(userValidator))
                        .AddValidatorsFromHolder(new AddressValidatorHolder(addressValidator))
                        .AddValidatorsFromHolder(new DetailsValidatorHolder(detailsValidator))
                    )
                };

                yield return new object[]
                {
                    "AddValidatorsFromHolder", userValidator, addressValidator, detailsValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidatorsFromHolder(new UserValidatorHolder(userValidator))
                        .AddValidatorsFromHolder(new AddressValidatorHolder(addressValidator))
                        .AddValidatorsFromHolder(new DetailsValidatorHolder(detailsValidator))
                    )
                };

                yield return new object[]
                {
                    "AddValidatorsFromHolder_AllInOne", userValidator, addressValidator, detailsValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidatorsFromHolder(new AllInOneValidatorHolder(userValidator, addressValidator, detailsValidator))
                    )
                };

                yield return new object[]
                {
                    "AddValidator_WithOverrides_FromHolder_AllInOne", userValidator, addressValidator, detailsValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator<User>(c => c)
                        .AddValidator<Address>(c => c)
                        .AddValidator<Details>(c => c)
                        .AddValidatorsFromHolder(new AllInOneValidatorHolder(userValidator, addressValidator, detailsValidator))
                    )
                };

                yield return new object[]
                {
                    "Mixed", userValidator, addressValidator, detailsValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator<User>(c => c)
                        .AddValidatorsFromHolder(new AddressValidatorHolder(c => c))
                        .AddValidatorsFromHolder(new AllInOneValidatorHolder(c => c, addressValidator, c => c))
                        .AddValidatorsFromHolder(new DetailsValidatorHolder(detailsValidator))
                        .AddValidator(userValidator)
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_AddValidator_Data))]
            public static void Create_Should_AddValidator(string debugName, Validator<User> userValidator, Validator<Address> addressValidator, Validator<Details> detailsValidator, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.NotNull(debugName);

                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);
                Assert.NotEmpty(coreValidator.Types);

                Assert.Equal(3, coreValidator.Types.Count);
                ValidationContextTestsHelper.AssertValidator(coreValidator, userValidator);
                ValidationContextTestsHelper.AssertValidator(coreValidator, addressValidator);
                ValidationContextTestsHelper.AssertValidator(coreValidator, detailsValidator);
            }

            [Fact]
            public void Create_Should_ThrowException_When_AddNullValidator()
            {
                Assert.Throws<ArgumentNullException>(() => new ValidationContextFactory().Create(o => o.AddValidator<User>(null)));
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

                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);

                Assert.Equal(NullRootStrategy.NoErrors, coreValidator.ValidationOptions.NullRootStrategy);
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
                        .SetValidationStategy(ValidationStrategy.Force)
                    )
                };

                yield return new object[]
                {
                    "Override",
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .SetValidationStategy(ValidationStrategy.Complete)
                        .SetValidationStategy(ValidationStrategy.Force)
                    )
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_SetValidationStrategy_Data))]
            public static void Create_Should_SetValidationStrategy(string debugName, Func<IValidationContextOptions, IValidationContextOptions> options)
            {
                Assert.NotNull(debugName);

                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);

                Assert.Equal(ValidationStrategy.Force, coreValidator.ValidationOptions.ValidationStrategy);
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

                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);

                Assert.Equal("[*]", coreValidator.ValidationOptions.CollectionForceKey);
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

                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);

                Assert.Equal(13, coreValidator.ValidationOptions.MaxDepth);
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

                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);

                Assert.Equal(message, coreValidator.ValidationOptions.RequiredError.Message);
                Assert.Equal("This is REQUIRED!", coreValidator.ValidationOptions.RequiredError.StringifiedMessage);
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

                var coreValidator = new ValidationContextFactory().Create(options);

                Assert.NotNull(coreValidator);

                Assert.Equal(message, coreValidator.ValidationOptions.RequiredError.Message);
                Assert.Equal("This is DEFAULT!", coreValidator.ValidationOptions.RequiredError.StringifiedMessage);
            }

            [Fact]
            public static void Create_Should_ThrowException_When_NullInDefaultErrorMessageArgs()
            {
                Assert.Throws<ArgumentNullException>(() => { new ValidationContextFactory().Create(options => options.SetDefaultError("test", new IMessageArg[] {null})); });
            }

            [Fact]
            public static void Create_Should_ThrowException_When_NullDefaultErrorMessage()
            {
                Assert.Throws<ArgumentNullException>(() => { new ValidationContextFactory().Create(options => options.SetDefaultError(null)); });
            }
        }

        [Fact]
        public void Create_Should_ProcessOptions()
        {
            var userValidator = new Validator<User>(m => m);

            var coreValidator = new ValidationContextFactory().Create(o => o
                .AddValidator(userValidator)
                .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                .SetNullRootStrategy(NullRootStrategy.NoErrors)
                .SetTranslationName("t1")
                .SetValidationStategy(ValidationStrategy.FailFast)
                .SetCollectionForceKey("[]")
                .SetMaxDepth(15)
                .SetRequiredError("This is required{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                .SetDefaultError("This is default{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
            );

            Assert.NotNull(coreValidator);

            Assert.Equal(1, coreValidator.Types.Count);
            ValidationContextTestsHelper.AssertValidator(coreValidator, userValidator);

            Assert.Equal("t1", coreValidator.Translations.Keys.Single());
            Assert.Single(coreValidator.Translations["t1"]);
            Assert.Equal("A", coreValidator.Translations["t1"]["a"]);

            Assert.Equal(NullRootStrategy.NoErrors, coreValidator.ValidationOptions.NullRootStrategy);
            Assert.Equal("t1", coreValidator.ValidationOptions.TranslationName);
            Assert.Equal(ValidationStrategy.FailFast, coreValidator.ValidationOptions.ValidationStrategy);
            Assert.Equal("[]", coreValidator.ValidationOptions.CollectionForceKey);
            Assert.Equal(15, coreValidator.ValidationOptions.MaxDepth);
            Assert.Equal("This is required{arg}", coreValidator.ValidationOptions.RequiredError.Message);
            Assert.Equal("This is required!", coreValidator.ValidationOptions.RequiredError.StringifiedMessage);
            Assert.Equal("This is default{arg}", coreValidator.ValidationOptions.DefaultError.Message);
            Assert.Equal("This is default!", coreValidator.ValidationOptions.DefaultError.StringifiedMessage);
        }

        [Fact]
        public void Create_Should_ProcessOptions_When_Default()
        {
            var coreValidator = new ValidationContextFactory().Create();

            Assert.NotNull(coreValidator);
            Assert.Empty(coreValidator.Translations);
            Assert.Empty(coreValidator.Types);

            Assert.Equal(NullRootStrategy.RequiredError, coreValidator.ValidationOptions.NullRootStrategy);
            Assert.Null(coreValidator.ValidationOptions.TranslationName);
            Assert.Equal(ValidationStrategy.Complete, coreValidator.ValidationOptions.ValidationStrategy);
            Assert.Equal("*", coreValidator.ValidationOptions.CollectionForceKey);
            Assert.Equal(10, coreValidator.ValidationOptions.MaxDepth);
            Assert.Equal("Required", coreValidator.ValidationOptions.RequiredError.Message);
            Assert.Equal("Required", coreValidator.ValidationOptions.RequiredError.StringifiedMessage);
            Assert.Equal("Invalid", coreValidator.ValidationOptions.DefaultError.Message);
            Assert.Equal("Invalid", coreValidator.ValidationOptions.DefaultError.StringifiedMessage);
        }

        [Fact]
        public void Create_Should_ProcessOptions_WithMoreAssignments()
        {
            var userValidator = new Validator<User>(m => m);
            var addressValidator = new Validator<Address>(m => m);

            var coreValidator = new ValidationContextFactory().Create(o => o
                .AddValidator(userValidator)
                .AddValidator(addressValidator)
                .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                .AddTranslation("t1", new Dictionary<string, string> {{"a1", "A1"}})
                .AddTranslation("t1", new Dictionary<string, string> {{"a2", "A2"}, {"a", "AA"}})
                .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                .SetNullRootStrategy(NullRootStrategy.NoErrors)
                .SetNullRootStrategy(NullRootStrategy.ArgumentNullException)
                .SetTranslationName("t1")
                .SetTranslationName("t2")
                .SetValidationStategy(ValidationStrategy.FailFast)
                .SetValidationStategy(ValidationStrategy.Force)
                .SetCollectionForceKey("[]")
                .SetCollectionForceKey("[*]")
                .SetMaxDepth(15)
                .SetMaxDepth(20)
                .SetRequiredError("This is required{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                .SetRequiredError("This is required{arg}. True story.", new IMessageArg[] {new MessageArg("arg", "!")})
                .SetDefaultError("This is default{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                .SetDefaultError("This is default{arg}. Legit.", new IMessageArg[] {new MessageArg("arg", "!")})
            );

            Assert.NotNull(coreValidator);
            Assert.Equal(2, coreValidator.Types.Count);

            ValidationContextTestsHelper.AssertValidator(coreValidator, userValidator);
            ValidationContextTestsHelper.AssertValidator(coreValidator, userValidator);

            Assert.Equal(2, coreValidator.Translations.Keys.Count());

            Assert.Equal(3, coreValidator.Translations["t1"].Count);
            Assert.Equal("AA", coreValidator.Translations["t1"]["a"]);
            Assert.Equal("A1", coreValidator.Translations["t1"]["a1"]);
            Assert.Equal("A2", coreValidator.Translations["t1"]["a2"]);

            Assert.Equal(1, coreValidator.Translations["t2"].Count);
            Assert.Equal("B", coreValidator.Translations["t2"]["b"]);

            Assert.Equal(NullRootStrategy.ArgumentNullException, coreValidator.ValidationOptions.NullRootStrategy);
            Assert.Equal("t2", coreValidator.ValidationOptions.TranslationName);
            Assert.Equal(ValidationStrategy.Force, coreValidator.ValidationOptions.ValidationStrategy);
            Assert.Equal("[*]", coreValidator.ValidationOptions.CollectionForceKey);
            Assert.Equal(20, coreValidator.ValidationOptions.MaxDepth);
            Assert.Equal("This is required{arg}. True story.", coreValidator.ValidationOptions.RequiredError.Message);
            Assert.Equal("This is required!. True story.", coreValidator.ValidationOptions.RequiredError.StringifiedMessage);
            Assert.Equal("This is default{arg}. Legit.", coreValidator.ValidationOptions.DefaultError.Message);
            Assert.Equal("This is default!. Legit.", coreValidator.ValidationOptions.DefaultError.StringifiedMessage);
        }

        [Fact]
        public void Create_Should_ThrowException_When_ReturningNewInstance()
        {
            Assert.Throws<InvalidProcessedReferenceException>(() => new ValidationContextFactory().Create(o => new ValidationContextOptions()));
        }
    }
}