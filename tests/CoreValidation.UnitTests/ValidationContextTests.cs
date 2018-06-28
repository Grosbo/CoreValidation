using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Exceptions;
using CoreValidation.Factory;
using CoreValidation.Options;
using CoreValidation.Translations;
using CoreValidation.Validators;
using Xunit;

// ReSharper disable ImplicitlyCapturedClosure

namespace CoreValidation.UnitTests
{
    public class CoreValidatorTests
    {
        public class User
        {
            public Address Address { get; set; } = new Address();
            public IEnumerable<Address> PastAddresses { get; set; } = new[] {new Address(), new Address()};
        }

        public class Address
        {
            public City City { get; set; } = new City();
        }

        public class City
        {
        }

        public class Validating
        {
            [Theory]
            [InlineData(0, true)]
            [InlineData(1, true)]
            [InlineData(2, false)]
            [InlineData(3, false)]
            public void Validate_Should_Validate_And_AllowOnlyMaxDepth(int maxDepth, bool expectException)
            {
                var options = new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.For(m => m.Address, m => m.ValidModel()))},
                        {typeof(Address), new Validator<Address>(c => c.For(m => m.City, m => m.ValidModel()))},
                        {typeof(City), new Validator<City>(c => c.Valid(m => false, "error"))}
                    }
                };

                ((ValidationOptions)options.ValidationOptions).MaxDepth = maxDepth;

                var validationContext = new ValidationContext(options);

                if (expectException)
                {
                    Assert.Throws<MaxDepthExceededException>(() => { validationContext.Validate(new User()); });
                }
                else
                {
                    validationContext.Validate(new User());
                }
            }

            [Theory]
            [InlineData(0, true)]
            [InlineData(1, true)]
            [InlineData(2, false)]
            [InlineData(3, false)]
            public void Validate_Should_Validate_And_AllowOnlyMaxDepth_When_MaxDepthOverriden(int maxDepth, bool expectException)
            {
                var options = new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.For(m => m.Address, m => m.ValidModel()))},
                        {typeof(Address), new Validator<Address>(c => c.For(m => m.City, m => m.ValidModel()))},
                        {typeof(City), new Validator<City>(c => c.Valid(m => false, "error"))}
                    }
                };

                ((ValidationOptions)options.ValidationOptions).MaxDepth = 3;

                var validationContext = new ValidationContext(options);

                if (expectException)
                {
                    Assert.Throws<MaxDepthExceededException>(() => { validationContext.Validate(new User(), o => o.SetMaxDepth(maxDepth)); });
                }
                else
                {
                    validationContext.Validate(new User());
                }
            }

            [Fact]
            public void Validate_Should_PassDefaultToTranslationsProxy()
            {
                var options = new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}},
                    Translations = new[]
                    {
                        new Translation("test1", new Dictionary<string, string> {{"t1", "T1"}}),
                        new Translation("test2", new Dictionary<string, string> {{"t2", "T2"}})
                    }
                };

                ((ValidationOptions)options.ValidationOptions).TranslationName = "test2";

                var validationContext = new ValidationContext(options);

                var result = validationContext.Validate(new User());

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.CoreValidatorId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.RulesOptions);
                Assert.False(result.ContainsMergedErrors);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Errors.Single().StringifiedMessage);

                Assert.Equal(validationContext.TranslatorsRepository, result.TranslationProxy.TranslatorsRepository);
                Assert.NotEqual(validationContext.TranslatorsRepository.GetOriginal(), result.TranslationProxy.DefaultTranslator);

                Assert.Equal(2, validationContext.TranslatorsRepository.Translations.Keys.Count());
                Assert.Equal(1, validationContext.TranslatorsRepository.Translations["test1"].Count);
                Assert.Equal("T1", validationContext.TranslatorsRepository.Translations["test1"]["t1"]);
                Assert.Equal(1, validationContext.TranslatorsRepository.Translations["test2"].Count);
                Assert.Equal("T2", validationContext.TranslatorsRepository.Translations["test2"]["t2"]);

                var errorT1 = new Error("t1");
                var errorT2 = new Error("t2");

                Assert.Equal("t1", result.TranslationProxy.DefaultTranslator(errorT1));
                Assert.Equal("t1", result.TranslationProxy.DefaultTranslator(errorT1));
                Assert.Equal("T2", result.TranslationProxy.DefaultTranslator(errorT2));
                Assert.Equal("T2", result.TranslationProxy.DefaultTranslator(errorT2));
            }

            [Fact]
            public void Validate_Should_PassDefaultToTranslationsProxy_When_TranslationNameDisabled()
            {
                var options = new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}},
                    Translations = new[]
                    {
                        new Translation("test1", new Dictionary<string, string> {{"t1", "T1"}}),
                        new Translation("test2", new Dictionary<string, string> {{"t2", "T2"}})
                    }
                };

                ((ValidationOptions)options.ValidationOptions).TranslationName = "test1";

                var validationContext = new ValidationContext(options);

                var result = validationContext.Validate(new User(), o => o.SetTranslationDisabled());

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.CoreValidatorId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.RulesOptions);
                Assert.False(result.ContainsMergedErrors);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Errors.Single().StringifiedMessage);

                Assert.Equal(validationContext.TranslatorsRepository, result.TranslationProxy.TranslatorsRepository);
                Assert.Equal(validationContext.TranslatorsRepository.GetOriginal(), result.TranslationProxy.DefaultTranslator);

                Assert.Equal(2, validationContext.TranslatorsRepository.Translations.Keys.Count());
                Assert.Equal(1, validationContext.TranslatorsRepository.Translations["test1"].Count);
                Assert.Equal("T1", validationContext.TranslatorsRepository.Translations["test1"]["t1"]);
                Assert.Equal(1, validationContext.TranslatorsRepository.Translations["test2"].Count);
                Assert.Equal("T2", validationContext.TranslatorsRepository.Translations["test2"]["t2"]);

                var errorT1 = new Error("t1");
                var errorT2 = new Error("t2");

                Assert.Equal("t1", result.TranslationProxy.DefaultTranslator(errorT1));
                Assert.Equal("t1", result.TranslationProxy.DefaultTranslator(errorT1));
                Assert.Equal("t2", result.TranslationProxy.DefaultTranslator(errorT2));
                Assert.Equal("t2", result.TranslationProxy.DefaultTranslator(errorT2));
            }

            [Fact]
            public void Validate_Should_PassDefaultToTranslationsProxy_When_TranslationNameOverriden()
            {
                var options = new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}},
                    Translations = new[]
                    {
                        new Translation("test1", new Dictionary<string, string> {{"t1", "T1"}}),
                        new Translation("test2", new Dictionary<string, string> {{"t2", "T2"}})
                    }
                };

                ((ValidationOptions)options.ValidationOptions).TranslationName = "test1";

                var validationContext = new ValidationContext(options);

                var result = validationContext.Validate(new User(), o => o.SetTranslationName("test2"));

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.CoreValidatorId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.RulesOptions);
                Assert.False(result.ContainsMergedErrors);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Errors.Single().StringifiedMessage);

                Assert.Equal(validationContext.TranslatorsRepository, result.TranslationProxy.TranslatorsRepository);
                Assert.NotEqual(validationContext.TranslatorsRepository.GetOriginal(), result.TranslationProxy.DefaultTranslator);

                Assert.Equal(2, validationContext.TranslatorsRepository.Translations.Keys.Count());
                Assert.Equal(1, validationContext.TranslatorsRepository.Translations["test1"].Count);
                Assert.Equal("T1", validationContext.TranslatorsRepository.Translations["test1"]["t1"]);
                Assert.Equal(1, validationContext.TranslatorsRepository.Translations["test2"].Count);
                Assert.Equal("T2", validationContext.TranslatorsRepository.Translations["test2"]["t2"]);

                var errorT1 = new Error("t1");
                var errorT2 = new Error("t2");

                Assert.Equal("t1", result.TranslationProxy.DefaultTranslator(errorT1));
                Assert.Equal("t1", result.TranslationProxy.DefaultTranslator(errorT1));
                Assert.Equal("T2", result.TranslationProxy.DefaultTranslator(errorT2));
                Assert.Equal("T2", result.TranslationProxy.DefaultTranslator(errorT2));
            }

            [Fact]
            public void Validate_Should_PassTranslationsProxy()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}},
                    Translations = new[]
                    {
                        new Translation("test1", new Dictionary<string, string> {{"t1", "T1"}}),
                        new Translation("test2", new Dictionary<string, string> {{"t2", "T2"}})
                    }
                });

                var result = validationContext.Validate(new User());

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.CoreValidatorId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.RulesOptions);
                Assert.False(result.ContainsMergedErrors);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Errors.Single().StringifiedMessage);

                Assert.Equal(validationContext.TranslatorsRepository, result.TranslationProxy.TranslatorsRepository);
                Assert.Equal(validationContext.TranslatorsRepository.GetOriginal(), result.TranslationProxy.DefaultTranslator);

                Assert.Equal(2, validationContext.TranslatorsRepository.Translations.Keys.Count());
                Assert.Equal(1, validationContext.TranslatorsRepository.Translations["test1"].Count);
                Assert.Equal("T1", validationContext.TranslatorsRepository.Translations["test1"]["t1"]);
                Assert.Equal(1, validationContext.TranslatorsRepository.Translations["test2"].Count);
                Assert.Equal("T2", validationContext.TranslatorsRepository.Translations["test2"]["t2"]);
            }

            [Fact]
            public void Validate_Should_PassValidationOptionsToModify()
            {
                var userValidator = new Validator<User>(m => m);

                var original = new ValidationContextFactory().Create(o => o
                    .AddValidator(userValidator)
                    .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                    .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                    .SetNullRootStrategy(NullRootStrategy.NoErrors)
                    .SetTranslationName("t1")
                    .SetValidationStategy(ValidationStrategy.FailFast)
                    .SetCollectionForceKey("[]")
                    .SetMaxDepth(15)
                    .SetRequiredError("This is required{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                );

                Assert.Equal(1, original.Types.Count);
                ValidationContextTestsHelper.AssertValidator(original, userValidator);

                Assert.Equal(2, original.Translations.Keys.Count());
                Assert.Equal(1, original.Translations["t1"].Count);
                Assert.Equal("A", original.Translations["t1"]["a"]);
                Assert.Equal(1, original.Translations["t2"].Count);
                Assert.Equal("B", original.Translations["t2"]["b"]);

                var compared = false;

                original.Validate(new User(), options =>
                    {
                        Assert.Equal(NullRootStrategy.NoErrors, options.NullRootStrategy);
                        Assert.Equal("t1", options.TranslationName);
                        Assert.Equal(ValidationStrategy.FailFast, options.ValidationStrategy);
                        Assert.Equal("[]", options.CollectionForceKey);
                        Assert.Equal(15, options.MaxDepth);
                        Assert.Equal("This is required{arg}", options.RequiredError.Message);
                        Assert.Equal("This is required!", options.RequiredError.StringifiedMessage);
                        compared = true;

                        return options;
                    }
                );

                Assert.True(compared);
            }

            [Fact]
            public void Validate_Should_ThrowException_When_NestedTypeNotRegistered()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.For(m => m.Address, m => m.ValidModel()))}}
                });

                Assert.Throws<ValidatorNotFoundException>(() => { validationContext.Validate(new User()); });
            }

            [Fact]
            public void Validate_Should_ThrowException_When_NoTypeRegistered()
            {
                var validationContext = new ValidationContext();

                Assert.Throws<ValidatorNotFoundException>(() => { validationContext.Validate(new User()); });
            }

            [Fact]
            public void Validate_Should_ThrowException_When_TypeNotRegistered()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(Address), new Validator<Address>(c => c)}}
                });

                Assert.Throws<ValidatorNotFoundException>(() => { validationContext.Validate(new User()); });
            }

            [Fact]
            public void Validate_Should_UseCollectionForceKey()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.For(m => m.PastAddresses, m => m.ValidCollection<User, IEnumerable<Address>, Address>(col => col.ValidModel())))},
                        {typeof(Address), new Validator<Address>(c => c.Valid(m => true, "error"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Force;
                ((ValidationOptions)validationContext.ValidationOptions).CollectionForceKey = "[*]";

                var result = validationContext.Validate(new User());
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Single(result.ErrorsCollection.Members);
                Assert.Equal(1, result.ErrorsCollection.Members["PastAddresses"].Members.Count);
                Assert.Equal("Required", result.ErrorsCollection.Members["PastAddresses"].Members["[*]"].Errors.ElementAt(0).Message);
                Assert.Equal("error", result.ErrorsCollection.Members["PastAddresses"].Members["[*]"].Errors.ElementAt(1).Message);
            }

            [Fact]
            public void Validate_Should_UseCollectionForceKey_When_CollectionForceKeyOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.For(m => m.PastAddresses, m => m.ValidCollection<User, IEnumerable<Address>, Address>(col => col.ValidModel())))},
                        {typeof(Address), new Validator<Address>(c => c.Valid(m => true, "error"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Force;
                ((ValidationOptions)validationContext.ValidationOptions).CollectionForceKey = "[*]";

                var result = validationContext.Validate(new User(), o => o.SetCollectionForceKey("XXX"));
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Single(result.ErrorsCollection.Members);
                Assert.Equal(1, result.ErrorsCollection.Members["PastAddresses"].Members.Count);
                Assert.Equal("Required", result.ErrorsCollection.Members["PastAddresses"].Members["XXX"].Errors.ElementAt(0).Message);
                Assert.Equal("error", result.ErrorsCollection.Members["PastAddresses"].Members["XXX"].Errors.ElementAt(1).Message);
            }

            [Fact]
            public void Validate_Should_Validate()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}}
                });

                var result = validationContext.Validate(new User());

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.CoreValidatorId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.RulesOptions);
                Assert.False(result.ContainsMergedErrors);
                Assert.Equal(validationContext.TranslatorsRepository, result.TranslationProxy.TranslatorsRepository);
                Assert.Equal(validationContext.TranslatorsRepository.GetOriginal(), result.TranslationProxy.DefaultTranslator);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Errors.Single().StringifiedMessage);
            }

            [Fact]
            public void Validate_Should_Validate_And_UseDefinedValidators()
            {
                var addressValidator = new Validator<Address>(c => c.Valid(m => false, "error 2 {arg2|case=lower}", new IMessageArg[] {new TextArg("arg2", "TEST2")}));

                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {
                            typeof(User), new Validator<User>(c => c
                                .For(m => m.Address, m => m.ValidModel(addressValidator))
                                .Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))
                        },
                        {typeof(Address), new Validator<Address>(c => c.Valid(m => false, "error 2 FROM_REPO {arg2|case=lower}", new IMessageArg[] {new TextArg("arg2", "TEST_FROM_REPO")}))}
                    }
                });

                var result = validationContext.Validate(new User());

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.CoreValidatorId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.RulesOptions);
                Assert.False(result.ContainsMergedErrors);
                Assert.Equal(validationContext.TranslatorsRepository, result.TranslationProxy.TranslatorsRepository);
                Assert.Equal(validationContext.TranslatorsRepository.GetOriginal(), result.TranslationProxy.DefaultTranslator);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Errors.Single().StringifiedMessage);

                Assert.Equal("error 2 {arg2|case=lower}", result.ErrorsCollection.Members["Address"].Errors.Single().Message);
                Assert.Equal("error 2 test2", result.ErrorsCollection.Members["Address"].Errors.Single().StringifiedMessage);
            }

            [Fact]
            public void Validate_Should_Validate_And_UseRepositoryValidators()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {
                            typeof(User), new Validator<User>(c => c
                                .For(m => m.Address, m => m.ValidModel())
                                .Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))
                        },
                        {typeof(Address), new Validator<Address>(c => c.Valid(m => false, "error 2 {arg2|case=lower}", new IMessageArg[] {new TextArg("arg2", "TEST2")}))}
                    }
                });

                var result = validationContext.Validate(new User());

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.CoreValidatorId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.RulesOptions);
                Assert.False(result.ContainsMergedErrors);
                Assert.Equal(validationContext.TranslatorsRepository, result.TranslationProxy.TranslatorsRepository);
                Assert.Equal(validationContext.TranslatorsRepository.GetOriginal(), result.TranslationProxy.DefaultTranslator);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Errors.Single().StringifiedMessage);

                Assert.Equal("error 2 {arg2|case=lower}", result.ErrorsCollection.Members["Address"].Errors.Single().Message);
                Assert.Equal("error 2 test2", result.ErrorsCollection.Members["Address"].Errors.Single().StringifiedMessage);
            }
        }

        public class SettingOptions
        {
            [Fact]
            public void Should_InitializeWithDefaultOptions()
            {
                var validationContext = new ValidationContext();

                Assert.NotNull(validationContext.Options);
                Assert.Empty(validationContext.Options.Translations);
                Assert.Empty(validationContext.Options.Validators);
                Assert.Equal(NullRootStrategy.RequiredError, validationContext.Options.ValidationOptions.NullRootStrategy);
                Assert.Equal(ValidationStrategy.Complete, validationContext.Options.ValidationOptions.ValidationStrategy);
                Assert.Null(validationContext.Options.ValidationOptions.TranslationName);
                Assert.Equal("*", validationContext.Options.ValidationOptions.CollectionForceKey);
                Assert.Equal(10, validationContext.Options.ValidationOptions.MaxDepth);
                Assert.Equal("Required", validationContext.Options.ValidationOptions.RequiredError.Message);
                Assert.Equal("Required", validationContext.Options.ValidationOptions.RequiredError.StringifiedMessage);
            }


            [Fact]
            public void Should_SetTranslations()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Translations = new[]
                    {
                        new Translation("test1", new Dictionary<string, string> {{"t1", "T1"}}),
                        new Translation("test2", new Dictionary<string, string> {{"t2", "T2"}})
                    }
                });

                Assert.Equal(2, validationContext.Translations.Keys.Count());
                Assert.Equal(1, validationContext.Translations["test1"].Count);
                Assert.Equal("T1", validationContext.Translations["test1"]["t1"]);
                Assert.Equal(1, validationContext.Translations["test2"].Count);
                Assert.Equal("T2", validationContext.Translations["test2"]["t2"]);
            }

            [Fact]
            public void Should_SetTypes()
            {
                var userValidator = new Validator<User>(m => m);
                var addressValidator = new Validator<Address>(m => m);

                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), userValidator},
                        {typeof(Address), addressValidator}
                    }
                });

                Assert.Equal(2, validationContext.Types.Count);
                ValidationContextTestsHelper.AssertValidator(validationContext, userValidator);
                ValidationContextTestsHelper.AssertValidator(validationContext, addressValidator);
            }
        }

        public class UsingNullStrategy
        {
            [Fact]
            public void Validate_Should_ReturnEmptyValidResult_If_NullRootStrategy_Is_NoErrors()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.NoErrors;
                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("This is required stuff {arg}", new IMessageArg[] {new TextArg("arg", "!")});

                var result = validationContext.Validate<User>(null);
                Assert.True(result.ErrorsCollection.IsEmpty);
            }

            [Fact]
            public void Validate_Should_ReturnEmptyValidResult_If_NullRootStrategy_Is_NoErrors_And_RootStrategyOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.ArgumentNullException;
                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("This is required stuff {arg}", new IMessageArg[] {new TextArg("arg", "!")});

                var result = validationContext.Validate<User>(null, o => o.SetNullRootStrategy(NullRootStrategy.NoErrors));
                Assert.True(result.ErrorsCollection.IsEmpty);
            }

            [Fact]
            public void Validate_Should_ReturnRequiredError_If_NullRootStrategy_Is_RequiredError()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.RequiredError;
                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("This is required stuff {arg}", new IMessageArg[] {new TextArg("arg", "!")});

                var result = validationContext.Validate<User>(null);

                Assert.Equal("This is required stuff {arg}", result.ErrorsCollection.Errors.Single().Message);
                Assert.Equal("This is required stuff !", result.ErrorsCollection.Errors.Single().StringifiedMessage);
            }

            [Fact]
            public void Validate_Should_ReturnRequiredError_If_NullRootStrategy_Is_RequiredError_And_RootStrategyOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.ArgumentNullException;
                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("REQUIRED!!!");

                var result = validationContext.Validate<User>(null, o => o
                    .SetNullRootStrategy(NullRootStrategy.RequiredError)
                    .SetRequiredError("This is required stuff {arg}", new IMessageArg[] {new TextArg("arg", "!")}));

                Assert.Equal("This is required stuff {arg}", result.ErrorsCollection.Errors.Single().Message);
                Assert.Equal("This is required stuff !", result.ErrorsCollection.Errors.Single().StringifiedMessage);
            }

            [Fact]
            public void Validate_Should_ThrowException_If_NullRootStrategy_Is_ArgumentNullException()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.ArgumentNullException;

                Assert.Throws<ArgumentNullException>(() => { validationContext.Validate<User>(null); });
            }

            [Fact]
            public void Validate_Should_ThrowException_If_NullRootStrategy_Is_ArgumentNullException_And_RootStrategyOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object> {{typeof(User), new Validator<User>(c => c.Valid(m => false, "error {arg|case=upper}", new IMessageArg[] {new TextArg("arg", "test")}))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.NoErrors;

                Assert.Throws<ArgumentNullException>(() => { validationContext.Validate<User>(null, o => o.SetNullRootStrategy(NullRootStrategy.ArgumentNullException)); });
            }
        }

        public class UsingValidationStrategy
        {
            [Fact]
            public void Validate_Should_Validate_Complete()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.Valid(m => true, "error 1").Valid(m => false, "error 2").Valid(m => false, "error 3"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Complete;

                var result = validationContext.Validate(new User());
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(2, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(0).StringifiedMessage);
                Assert.Equal("error 3", result.ErrorsCollection.Errors.ElementAt(1).StringifiedMessage);
            }

            [Fact]
            public void Validate_Should_Validate_Complete_WhenStrategyIsOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.Valid(m => true, "error 1").Valid(m => false, "error 2").Valid(m => false, "error 3"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Force;

                var result = validationContext.Validate(new User(), o => o.SetValidationStategy(ValidationStrategy.Complete));
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(2, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(0).StringifiedMessage);
                Assert.Equal("error 3", result.ErrorsCollection.Errors.ElementAt(1).StringifiedMessage);
            }

            [Fact]
            public void Validate_Should_Validate_FailFast()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.Valid(m => true, "error 1").Valid(m => false, "error 2").Valid(m => false, "error 3"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.FailFast;

                var result = validationContext.Validate(new User());
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(1, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(0).StringifiedMessage);
            }

            [Fact]
            public void Validate_Should_Validate_FailFast_When_StrategyIsOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.Valid(m => true, "error 1").Valid(m => false, "error 2").Valid(m => false, "error 3"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Force;

                var result = validationContext.Validate(new User(), o => o.SetValidationStategy(ValidationStrategy.FailFast));
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(1, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(0).StringifiedMessage);
            }

            [Fact]
            public void Validate_Should_Validate_Force()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.Valid(m => true, "error 1").Valid(m => false, "error 2").Valid(m => false, "error 3"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Force;

                var result = validationContext.Validate(new User());
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(3, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 1", result.ErrorsCollection.Errors.ElementAt(0).StringifiedMessage);
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(1).StringifiedMessage);
                Assert.Equal("error 3", result.ErrorsCollection.Errors.ElementAt(2).StringifiedMessage);
            }

            [Fact]
            public void Validate_Should_Validate_Force_When_StrategyIsOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.Valid(m => true, "error 1").Valid(m => false, "error 2").Valid(m => false, "error 3"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Complete;

                var result = validationContext.Validate(new User(), o => o.SetValidationStategy(ValidationStrategy.Force));
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(3, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 1", result.ErrorsCollection.Errors.ElementAt(0).StringifiedMessage);
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(1).StringifiedMessage);
                Assert.Equal("error 3", result.ErrorsCollection.Errors.ElementAt(2).StringifiedMessage);
            }
        }


        public class UsingRequiredError
        {
            [Fact]
            public void Validate_Should_ReturnRequiredError_When_NullRequiredMember()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.For(m => m.Address, m => m.ValidModel()))},
                        {typeof(Address), new Validator<Address>(c => c.Valid(m => false, "error 2"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("Required {arg}", new[] {new TextArg("arg", "!!!")});

                var result = validationContext.Validate(new User {Address = null});

                Assert.Equal(0, result.ErrorsCollection.Errors.Count);
                Assert.Equal(1, result.ErrorsCollection.Members.Count);
                Assert.Equal("Required {arg}", result.ErrorsCollection.Members["Address"].Errors.Single().Message);
                Assert.Equal("Required !!!", result.ErrorsCollection.Members["Address"].Errors.Single().StringifiedMessage);
            }

            [Fact]
            public void Validate_Should_ReturnRequiredError_When_NullRequiredMember_And_RequiredErrorOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Validators = new Dictionary<Type, object>
                    {
                        {typeof(User), new Validator<User>(c => c.For(m => m.Address, m => m.ValidModel()))},
                        {typeof(Address), new Validator<Address>(c => c.Valid(m => false, "error 2"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("Required !!!");

                var result = validationContext.Validate(new User {Address = null}, o => o.SetRequiredError("Required {arg}", new IMessageArg[] {new TextArg("arg", "!!!")}));

                Assert.Equal(0, result.ErrorsCollection.Errors.Count);
                Assert.Equal(1, result.ErrorsCollection.Members.Count);
                Assert.Equal("Required {arg}", result.ErrorsCollection.Members["Address"].Errors.Single().Message);
                Assert.Equal("Required !!!", result.ErrorsCollection.Members["Address"].Errors.Single().StringifiedMessage);
            }
        }

        public class Cloning
        {
            public static IEnumerable<object[]> Create_Should_AddValues_Data()
            {
                var userValidator = new Validator<User>(c => c);
                var addressValidator = new Validator<Address>(c => c);

                yield return new object[]
                {
                    "ToEmpty", userValidator, addressValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o),
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator(userValidator)
                        .AddValidator(addressValidator)
                        .AddTranslation("t1", new Dictionary<string, string>
                        {
                            {"a", "A"},
                            {"a1", "A1"},
                            {"a2", "A2"}
                        })
                        .AddTranslation("t2", new Dictionary<string, string>
                        {
                            {"b", "B"}
                        }))
                };

                yield return new object[]
                {
                    "AddEmpty", userValidator, addressValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator(userValidator)
                        .AddValidator(addressValidator)
                        .AddTranslation("t1", new Dictionary<string, string>
                        {
                            {"a", "A"},
                            {"a1", "A1"},
                            {"a2", "A2"}
                        })
                        .AddTranslation("t2", new Dictionary<string, string>
                        {
                            {"b", "B"}
                        })),
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o)
                };

                yield return new object[]
                {
                    "AddValues", userValidator, addressValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator(userValidator)
                        .AddTranslation("t1", new Dictionary<string, string>
                        {
                            {"a", "A"},
                            {"a1", "A1"},
                            {"a2", "A2"}
                        })),
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator(addressValidator)
                        .AddTranslation("t2", new Dictionary<string, string>
                        {
                            {"b", "B"}
                        }))
                };

                yield return new object[]
                {
                    "OverrideValues", userValidator, addressValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator<User>(c => c)
                        .AddValidator(addressValidator)
                        .AddTranslation("t1", new Dictionary<string, string>
                        {
                            {"a", "A"},
                            {"a1", "A1x"}
                        })
                        .AddTranslation("t2", new Dictionary<string, string>
                        {
                            {"b", "Bx"}
                        })),
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator(userValidator)
                        .AddTranslation("t1", new Dictionary<string, string>
                        {
                            {"a1", "A1"},
                            {"a2", "A2"}
                        })
                        .AddTranslation("t2", new Dictionary<string, string>
                        {
                            {"b", "B"}
                        }))
                };

                yield return new object[]
                {
                    "Duplicates", userValidator, addressValidator,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator(userValidator)
                        .AddValidator(addressValidator)
                        .AddTranslation("t1", new Dictionary<string, string>
                        {
                            {"a", "A"},
                            {"a1", "A1"},
                            {"a2", "A2"}
                        })
                        .AddTranslation("t2", new Dictionary<string, string>
                        {
                            {"b", "B"}
                        })),
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddValidator(userValidator)
                        .AddValidator(addressValidator)
                        .AddTranslation("t1", new Dictionary<string, string>
                        {
                            {"a", "A"},
                            {"a1", "A1"},
                            {"a2", "A2"}
                        })
                        .AddTranslation("t2", new Dictionary<string, string>
                        {
                            {"b", "B"}
                        }))
                };
            }

            [Theory]
            [MemberData(nameof(Create_Should_AddValues_Data))]
            public static void Create_Should_AddValues(string debugName, Validator<User> userValidator, Validator<Address> addressValidator, Func<IValidationContextOptions, IValidationContextOptions> original, Func<IValidationContextOptions, IValidationContextOptions> clone)
            {
                Assert.NotNull(debugName);

                var originalValidator = new ValidationContextFactory().Create(original);

                var clonedValidator = originalValidator.Clone(clone);

                Assert.Equal(2, clonedValidator.Types.Count);
                ValidationContextTestsHelper.AssertValidator(clonedValidator, userValidator);
                ValidationContextTestsHelper.AssertValidator(clonedValidator, addressValidator);

                Assert.Equal(2, clonedValidator.Translations.Keys.Count());
                Assert.Equal(3, clonedValidator.Translations["t1"].Count);
                Assert.Equal("A", clonedValidator.Translations["t1"]["a"]);
                Assert.Equal("A1", clonedValidator.Translations["t1"]["a1"]);
                Assert.Equal("A2", clonedValidator.Translations["t1"]["a2"]);
                Assert.Equal(1, clonedValidator.Translations["t2"].Count);
                Assert.Equal("B", clonedValidator.Translations["t2"]["b"]);
            }

            [Fact]
            public void Clone_Should_ReplaceValues()
            {
                var userValidator = new Validator<User>(m => m);
                var userValidator2 = new Validator<User>(m => m);

                var original = new ValidationContextFactory().Create(o => o
                    .AddValidator(userValidator)
                    .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                    .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                    .SetNullRootStrategy(NullRootStrategy.NoErrors)
                    .SetTranslationName("t1")
                    .SetValidationStategy(ValidationStrategy.FailFast)
                    .SetCollectionForceKey("[]")
                    .SetMaxDepth(15)
                    .SetRequiredError("This is required{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                );

                Assert.Equal(1, original.Types.Count);
                ValidationContextTestsHelper.AssertValidator(original, userValidator);

                Assert.Equal(2, original.Translations.Keys.Count());
                Assert.Equal(1, original.Translations["t1"].Count);
                Assert.Equal("A", original.Translations["t1"]["a"]);
                Assert.Equal(1, original.Translations["t2"].Count);
                Assert.Equal("B", original.Translations["t2"]["b"]);

                Assert.Equal(NullRootStrategy.NoErrors, original.ValidationOptions.NullRootStrategy);
                Assert.Equal("t1", original.ValidationOptions.TranslationName);
                Assert.Equal(ValidationStrategy.FailFast, original.ValidationOptions.ValidationStrategy);
                Assert.Equal("[]", original.ValidationOptions.CollectionForceKey);
                Assert.Equal(15, original.ValidationOptions.MaxDepth);
                Assert.Equal("This is required{arg}", original.ValidationOptions.RequiredError.Message);
                Assert.Equal("This is required!", original.ValidationOptions.RequiredError.StringifiedMessage);

                var clone = original.Clone(o => o
                    .AddValidator(userValidator2)
                    .AddTranslation("t1", new Dictionary<string, string> {{"a", "AA"}})
                    .AddTranslation("t2", new Dictionary<string, string> {{"b", "BB"}})
                    .SetNullRootStrategy(NullRootStrategy.ArgumentNullException)
                    .SetTranslationDisabled()
                    .SetValidationStategy(ValidationStrategy.Force)
                    .SetCollectionForceKey("[*]")
                    .SetMaxDepth(20)
                    .SetRequiredError("This is required{arg} !!!", new IMessageArg[] {new MessageArg("arg", "!")})
                );

                Assert.NotNull(original);
                Assert.NotNull(clone);
                Assert.NotSame(original, clone);

                Assert.Equal(1, clone.Types.Count);
                ValidationContextTestsHelper.AssertValidator(clone, userValidator2);

                Assert.NotSame(original.Translations, clone.Translations);

                Assert.Equal(2, original.Translations.Keys.Count());
                Assert.Equal(1, original.Translations["t1"].Count);
                Assert.Equal("AA", original.Translations["t1"]["a"]);
                Assert.Equal(1, original.Translations["t2"].Count);
                Assert.Equal("BB", original.Translations["t2"]["b"]);

                Assert.NotSame(original.Translations["t1"], clone.Translations["t1"]);
                Assert.NotSame(original.Translations["t2"], clone.Translations["t2"]);

                Assert.Equal(NullRootStrategy.ArgumentNullException, clone.ValidationOptions.NullRootStrategy);
                Assert.Null(clone.ValidationOptions.TranslationName);
                Assert.Equal(ValidationStrategy.Force, clone.ValidationOptions.ValidationStrategy);
                Assert.Equal("[*]", clone.ValidationOptions.CollectionForceKey);
                Assert.Equal(20, clone.ValidationOptions.MaxDepth);
                Assert.Equal("This is required{arg} !!!", clone.ValidationOptions.RequiredError.Message);
                Assert.Equal("This is required! !!!", clone.ValidationOptions.RequiredError.StringifiedMessage);

                Assert.NotSame(original.ValidationOptions, clone.ValidationOptions);
                Assert.NotSame(original.ValidationOptions.RequiredError, clone.ValidationOptions.RequiredError);
            }

            [Fact]
            public void Clone_Should_ReturnExactCopy()
            {
                var userValidator = new Validator<User>(m => m);

                var original = new ValidationContextFactory().Create(o => o
                    .AddValidator(userValidator)
                    .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                    .SetNullRootStrategy(NullRootStrategy.NoErrors)
                    .SetTranslationName("t1")
                    .SetValidationStategy(ValidationStrategy.FailFast)
                    .SetCollectionForceKey("[]")
                    .SetMaxDepth(15)
                    .SetRequiredError("This is required{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                );

                var clone = original.Clone();

                Assert.NotNull(original);
                Assert.NotNull(clone);
                Assert.NotSame(original, clone);

                void CheckValidatorsVariables(IValidationContext validationContext)
                {
                    Assert.Equal(1, validationContext.Types.Count);
                    ValidationContextTestsHelper.AssertValidator(validationContext, userValidator);

                    Assert.Equal("t1", validationContext.Translations.Keys.Single());
                    Assert.Single(validationContext.Translations["t1"]);
                    Assert.Equal("A", validationContext.Translations["t1"]["a"]);

                    Assert.Equal(NullRootStrategy.NoErrors, validationContext.ValidationOptions.NullRootStrategy);
                    Assert.Equal("t1", validationContext.ValidationOptions.TranslationName);
                    Assert.Equal(ValidationStrategy.FailFast, validationContext.ValidationOptions.ValidationStrategy);
                    Assert.Equal("[]", validationContext.ValidationOptions.CollectionForceKey);
                    Assert.Equal(15, validationContext.ValidationOptions.MaxDepth);
                    Assert.Equal("This is required{arg}", validationContext.ValidationOptions.RequiredError.Message);
                    Assert.Equal("This is required!", validationContext.ValidationOptions.RequiredError.StringifiedMessage);
                }

                CheckValidatorsVariables(original);
                CheckValidatorsVariables(clone);
            }
        }

        [Fact]
        public void Should_SetId()
        {
            var ids = new List<Guid>();

            for (var i = 0; i < 10; ++i)
            {
                var validationContext = new ValidationContext(new ValidationContextOptions());

                ids.Add(validationContext.Id);
            }

            Assert.Equal(10, ids.Distinct().Count());
        }
    }
}