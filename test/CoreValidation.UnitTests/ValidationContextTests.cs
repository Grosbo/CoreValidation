using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Factory;
using CoreValidation.Options;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

// ReSharper disable ImplicitlyCapturedClosure

namespace CoreValidation.UnitTests
{
    public class CoreValidatorTests
    {
        public class User
        {
            public string Name { get; set; }
            public Address Address { get; set; } = new Address();
            public IEnumerable<Address> PastAddresses { get; set; } = new[] {new Address(), new Address()};
        }

        public class Address
        {
            public string Street { get; set; }
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
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c.Member(m => m.Address, m => m.AsModel()))},
                        {typeof(Address), new Specification<Address>(c => c.Member(m => m.City, m => m.AsModel()))},
                        {typeof(City), new Specification<City>(c => c.Valid(m => false))}
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
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c.Member(m => m.Address, m => m.AsModel()))},
                        {typeof(Address), new Specification<Address>(c => c.Member(m => m.City, m => m.AsModel()))},
                        {typeof(City), new Specification<City>(c => c.Valid(m => false))}
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
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Member(m => m.Name, m => m.Valid(n => false, "error {arg|case=upper}", new[] {Arg.Text("arg", "test")})))}},
                    Translations = new[]
                    {
                        new Translation("test1", new Dictionary<string, string> {{"t1", "T1"}}),
                        new Translation("test2", new Dictionary<string, string> {{"t2", "T2"}})
                    }
                };

                ((ValidationOptions)options.ValidationOptions).TranslationName = "test2";

                var validationContext = new ValidationContext(options);

                var result = validationContext.Validate(new User {Name = ""});

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.ValidationContextId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.ExecutionOptions);
                Assert.False(result.IsMergeResult);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Empty(result.ErrorsCollection.Errors);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Members["Name"].Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Members["Name"].Errors.Single().ToFormattedMessage());

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
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Member(m => m.Name, m => m.Valid(n => false, "error {arg|case=upper}", new[] {Arg.Text("arg", "test")})))}},
                    Translations = new[]
                    {
                        new Translation("test1", new Dictionary<string, string> {{"t1", "T1"}}),
                        new Translation("test2", new Dictionary<string, string> {{"t2", "T2"}})
                    }
                };

                ((ValidationOptions)options.ValidationOptions).TranslationName = "test1";

                var validationContext = new ValidationContext(options);

                var result = validationContext.Validate(new User {Name = ""}, o => o.SetTranslationDisabled());

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.ValidationContextId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.ExecutionOptions);
                Assert.False(result.IsMergeResult);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Empty(result.ErrorsCollection.Errors);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Members["Name"].Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Members["Name"].Errors.Single().ToFormattedMessage());

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
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Member(m => m.Name, m => m.Valid(n => false, "error {arg|case=upper}", new[] {Arg.Text("arg", "test")})))}},
                    Translations = new[]
                    {
                        new Translation("test1", new Dictionary<string, string> {{"t1", "T1"}}),
                        new Translation("test2", new Dictionary<string, string> {{"t2", "T2"}})
                    }
                };

                ((ValidationOptions)options.ValidationOptions).TranslationName = "test1";

                var validationContext = new ValidationContext(options);

                var result = validationContext.Validate(new User {Name = ""}, o => o.SetTranslationName("test2"));

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.ValidationContextId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.ExecutionOptions);
                Assert.False(result.IsMergeResult);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Empty(result.ErrorsCollection.Errors);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Members["Name"].Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Members["Name"].Errors.Single().ToFormattedMessage());

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
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Member(m => m.Name, m => m.Valid(n => false, "error {arg|case=upper}", new[] {Arg.Text("arg", "test")})))}},
                    Translations = new[]
                    {
                        new Translation("test1", new Dictionary<string, string> {{"t1", "T1"}}),
                        new Translation("test2", new Dictionary<string, string> {{"t2", "T2"}})
                    }
                });

                var result = validationContext.Validate(new User {Name = ""});

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.ValidationContextId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.ExecutionOptions);
                Assert.False(result.IsMergeResult);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Empty(result.ErrorsCollection.Errors);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Members["Name"].Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Members["Name"].Errors.Single().ToFormattedMessage());

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
                var userSpecification = new Specification<User>(m => m);

                var original = new ValidationContextFactory().Create(o => o
                    .AddSpecification(userSpecification)
                    .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                    .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                    .SetNullRootStrategy(NullRootStrategy.NoErrors)
                    .SetTranslationName("t1")
                    .SetValidationStrategy(ValidationStrategy.FailFast)
                    .SetCollectionForceKey("[]")
                    .SetMaxDepth(15)
                    .SetRequiredError("This is required")
                );

                Assert.Equal(1, original.Types.Count);
                ValidationContextTestsHelper.AssertSpecification(original, userSpecification);

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
                        Assert.Equal("This is required", options.RequiredError.Message);
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
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Member(m => m.Address, m => m.AsModel()))}}
                });

                var exception = Assert.Throws<SpecificationNotFoundException>(() => { validationContext.Validate(new User()); });

                Assert.Equal(typeof(Address), exception.Type);
            }

            [Fact]
            public void Validate_Should_ThrowException_When_NoTypeRegistered()
            {
                var validationContext = new ValidationContext();

                var exception = Assert.Throws<SpecificationNotFoundException>(() => { validationContext.Validate(new User()); });

                Assert.Equal(typeof(User), exception.Type);
            }

            [Fact]
            public void Validate_Should_ThrowException_When_TypeNotRegistered()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object> {{typeof(Address), new Specification<Address>(c => c)}}
                });

                var exception = Assert.Throws<SpecificationNotFoundException>(() => { validationContext.Validate(new User()); });

                Assert.Equal(typeof(User), exception.Type);
            }

            [Fact]
            public void Validate_Should_UseCollectionForceKey()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c.Member(m => m.PastAddresses, m => m.AsCollection<User, IEnumerable<Address>, Address>(col => col.AsModel())))},
                        {typeof(Address), new Specification<Address>(c => c.Valid(m => true).WithMessage("error"))}
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
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c.Member(m => m.PastAddresses, m => m.AsCollection<User, IEnumerable<Address>, Address>(col => col.AsModel())))},
                        {typeof(Address), new Specification<Address>(c => c.Valid(m => true).WithMessage("error"))}
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
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Member(m => m.Name, m => m.Valid(v => false, "error {arg|case=upper}", new[] {Arg.Text("arg", "test")})))}}
                });

                var result = validationContext.Validate(new User {Name = ""});

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.ValidationContextId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.ExecutionOptions);
                Assert.False(result.IsMergeResult);
                Assert.Equal(validationContext.TranslatorsRepository, result.TranslationProxy.TranslatorsRepository);
                Assert.Equal(validationContext.TranslatorsRepository.GetOriginal(), result.TranslationProxy.DefaultTranslator);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Empty(result.ErrorsCollection.Errors);
                Assert.Equal("error {arg|case=upper}", result.ErrorsCollection.Members["Name"].Errors.Single().Message);
                Assert.Equal("error TEST", result.ErrorsCollection.Members["Name"].Errors.Single().ToFormattedMessage());
            }

            [Fact]
            public void Validate_Should_Validate_And_UseDefinedSpecification()
            {
                var addressSpecification = new Specification<Address>(c => c.Member(m => m.Street, m => m.Valid(n => false, "error 2 DEFINED {arg2|case=lower}", new[] {Arg.Text("arg2", "TEST2")})));

                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {
                            typeof(User), new Specification<User>(c => c
                                .Member(m => m.Address, m => m.AsModel(addressSpecification)))
                        },
                        {typeof(Address), new Specification<Address>(c => c.Member(m => m.Street, m => m.Valid(n => false, "error 2 FROM REPO {arg2|case=lower}", new[] {Arg.Text("arg2", "TEST2")})))}
                    }
                });

                var result = validationContext.Validate(new User {Address = new Address {Street = ""}});

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.ValidationContextId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.ExecutionOptions);
                Assert.False(result.IsMergeResult);
                Assert.Equal(validationContext.TranslatorsRepository, result.TranslationProxy.TranslatorsRepository);
                Assert.Equal(validationContext.TranslatorsRepository.GetOriginal(), result.TranslationProxy.DefaultTranslator);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Empty(result.ErrorsCollection.Errors);
                Assert.Equal("error 2 DEFINED {arg2|case=lower}", result.ErrorsCollection.Members["Address"].Members["Street"].Errors.Single().Message);
                Assert.Equal("error 2 DEFINED test2", result.ErrorsCollection.Members["Address"].Members["Street"].Errors.Single().ToFormattedMessage());
            }

            [Fact]
            public void Validate_Should_Validate_And_UseRepositorySpecifications()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {
                            typeof(User), new Specification<User>(c => c
                                .Member(m => m.Address, m => m.AsModel()))
                        },
                        {typeof(Address), new Specification<Address>(c => c.Member(m => m.Street, m => m.Valid(n => false, "error 2 FROM REPO {arg2|case=lower}", new[] {Arg.Text("arg2", "TEST2")})))}
                    }
                });

                var result = validationContext.Validate(new User {Address = new Address {Street = ""}});

                Assert.NotNull(result);
                Assert.Equal(validationContext.Id, result.ValidationContextId);
                Assert.Equal(validationContext.Options.ValidationOptions, result.ExecutionOptions);
                Assert.False(result.IsMergeResult);
                Assert.Equal(validationContext.TranslatorsRepository, result.TranslationProxy.TranslatorsRepository);
                Assert.Equal(validationContext.TranslatorsRepository.GetOriginal(), result.TranslationProxy.DefaultTranslator);
                Assert.NotNull(result.ErrorsCollection);
                Assert.Empty(result.ErrorsCollection.Errors);
                Assert.Equal("error 2 FROM REPO {arg2|case=lower}", result.ErrorsCollection.Members["Address"].Members["Street"].Errors.Single().Message);
                Assert.Equal("error 2 FROM REPO test2", result.ErrorsCollection.Members["Address"].Members["Street"].Errors.Single().ToFormattedMessage());
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
                Assert.Empty(validationContext.Options.Specifications);
                Assert.Equal(NullRootStrategy.RequiredError, validationContext.Options.ValidationOptions.NullRootStrategy);
                Assert.Equal(ValidationStrategy.Complete, validationContext.Options.ValidationOptions.ValidationStrategy);
                Assert.Null(validationContext.Options.ValidationOptions.TranslationName);
                Assert.Equal("*", validationContext.Options.ValidationOptions.CollectionForceKey);
                Assert.Equal(10, validationContext.Options.ValidationOptions.MaxDepth);
                Assert.Equal("Required", validationContext.Options.ValidationOptions.RequiredError.Message);
                Assert.Equal("Required", validationContext.Options.ValidationOptions.RequiredError.ToFormattedMessage());
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
                var userSpecification = new Specification<User>(m => m);
                var addressSpecification = new Specification<Address>(m => m);

                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), userSpecification},
                        {typeof(Address), addressSpecification}
                    }
                });

                Assert.Equal(2, validationContext.Types.Count);
                ValidationContextTestsHelper.AssertSpecification(validationContext, userSpecification);
                ValidationContextTestsHelper.AssertSpecification(validationContext, addressSpecification);
            }
        }

        public class UsingNullStrategy
        {
            [Fact]
            public void Validate_Should_ReturnEmptyValidResult_If_NullRootStrategy_Is_NoErrors()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Valid(m => false).WithMessage("error"))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.NoErrors;
                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("This is required stuff {arg}", new[] {Arg.Text("arg", "!")});

                var result = validationContext.Validate<User>(null);
                Assert.True(result.ErrorsCollection.IsEmpty);
            }

            [Fact]
            public void Validate_Should_ReturnEmptyValidResult_If_NullRootStrategy_Is_NoErrors_And_RootStrategyOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Valid(m => false).WithMessage("error"))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.ArgumentNullException;
                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("This is required stuff {arg}", new[] {Arg.Text("arg", "!")});

                var result = validationContext.Validate<User>(null, o => o.SetNullRootStrategy(NullRootStrategy.NoErrors));
                Assert.True(result.ErrorsCollection.IsEmpty);
            }

            [Fact]
            public void Validate_Should_ReturnRequiredError_If_NullRootStrategy_Is_RequiredError()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Valid(m => false).WithMessage("error"))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.RequiredError;
                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("This is required");

                var result = validationContext.Validate<User>(null);

                Assert.Equal("This is required", result.ErrorsCollection.Errors.Single().Message);
            }

            [Fact]
            public void Validate_Should_ReturnRequiredError_If_NullRootStrategy_Is_RequiredError_And_RootStrategyOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Valid(m => false).WithMessage("error"))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.ArgumentNullException;
                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("REQUIRED!!!");

                var result = validationContext.Validate<User>(null, o => o
                    .SetNullRootStrategy(NullRootStrategy.RequiredError)
                    .SetRequiredError("This is required stuff"));

                Assert.Equal("This is required stuff", result.ErrorsCollection.Errors.Single().Message);
            }

            [Fact]
            public void Validate_Should_ThrowException_If_NullRootStrategy_Is_ArgumentNullException()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Valid(m => false).WithMessage("error"))}}
                });

                ((ValidationOptions)validationContext.ValidationOptions).NullRootStrategy = NullRootStrategy.ArgumentNullException;

                Assert.Throws<ArgumentNullException>(() => { validationContext.Validate<User>(null); });
            }

            [Fact]
            public void Validate_Should_ThrowException_If_NullRootStrategy_Is_ArgumentNullException_And_RootStrategyOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object> {{typeof(User), new Specification<User>(c => c.Valid(m => false).WithMessage("error"))}}
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
                    Specifications = new Dictionary<Type, object>
                    {
                        {
                            typeof(User), new Specification<User>(c => c
                                .Valid(m => true).WithMessage("error 1")
                                .Valid(m => false).WithMessage("error 2")
                                .Valid(m => false).WithMessage("error 3"))
                        }
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Complete;

                var result = validationContext.Validate(new User());
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(2, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(0).ToFormattedMessage());
                Assert.Equal("error 3", result.ErrorsCollection.Errors.ElementAt(1).ToFormattedMessage());
            }

            [Fact]
            public void Validate_Should_Validate_Complete_WhenStrategyIsOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {
                            typeof(User), new Specification<User>(c => c
                                .Valid(m => true).WithMessage("error 1")
                                .Valid(m => false).WithMessage("error 2")
                                .Valid(m => false).WithMessage("error 3"))
                        }
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Force;

                var result = validationContext.Validate(new User(), o => o.SetValidationStrategy(ValidationStrategy.Complete));
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(2, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(0).ToFormattedMessage());
                Assert.Equal("error 3", result.ErrorsCollection.Errors.ElementAt(1).ToFormattedMessage());
            }

            [Fact]
            public void Validate_Should_Validate_FailFast()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {
                            typeof(User), new Specification<User>(c => c
                                .Valid(m => true).WithMessage("error 1")
                                .Valid(m => false).WithMessage("error 2")
                                .Valid(m => false).WithMessage("error 3"))
                        }
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.FailFast;

                var result = validationContext.Validate(new User());
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(1, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(0).ToFormattedMessage());
            }

            [Fact]
            public void Validate_Should_Validate_FailFast_When_StrategyIsOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {
                            typeof(User), new Specification<User>(c => c
                                .Valid(m => true).WithMessage("error 1")
                                .Valid(m => false).WithMessage("error 2")
                                .Valid(m => false).WithMessage("error 3"))
                        }
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Force;

                var result = validationContext.Validate(new User(), o => o.SetValidationStrategy(ValidationStrategy.FailFast));
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(1, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(0).ToFormattedMessage());
            }

            [Fact]
            public void Validate_Should_Validate_Force()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {
                            typeof(User), new Specification<User>(c => c
                                .Valid(m => true).WithMessage("error 1")
                                .Valid(m => false).WithMessage("error 2")
                                .Valid(m => false).WithMessage("error 3"))
                        }
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Force;

                var result = validationContext.Validate(new User());
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(3, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 1", result.ErrorsCollection.Errors.ElementAt(0).ToFormattedMessage());
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(1).ToFormattedMessage());
                Assert.Equal("error 3", result.ErrorsCollection.Errors.ElementAt(2).ToFormattedMessage());
            }

            [Fact]
            public void Validate_Should_Validate_Force_When_StrategyIsOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {
                            typeof(User), new Specification<User>(c => c
                                .Valid(m => true).WithMessage("error 1")
                                .Valid(m => false).WithMessage("error 2")
                                .Valid(m => false).WithMessage("error 3"))
                        }
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).ValidationStrategy = ValidationStrategy.Complete;

                var result = validationContext.Validate(new User(), o => o.SetValidationStrategy(ValidationStrategy.Force));
                Assert.False(result.ErrorsCollection.IsEmpty);
                Assert.Equal(3, result.ErrorsCollection.Errors.Count);
                Assert.Equal("error 1", result.ErrorsCollection.Errors.ElementAt(0).ToFormattedMessage());
                Assert.Equal("error 2", result.ErrorsCollection.Errors.ElementAt(1).ToFormattedMessage());
                Assert.Equal("error 3", result.ErrorsCollection.Errors.ElementAt(2).ToFormattedMessage());
            }
        }

        public class UsingRequiredError
        {
            [Fact]
            public void Validate_Should_ReturnRequiredError_When_NullRequiredMember()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c.Member(m => m.Address, m => m.AsModel()))},
                        {typeof(Address), new Specification<Address>(c => c.Valid(m => false).WithMessage("error 2"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("Required !!!");

                var result = validationContext.Validate(new User {Address = null});

                Assert.Equal(0, result.ErrorsCollection.Errors.Count);
                Assert.Equal(1, result.ErrorsCollection.Members.Count);
                Assert.Equal("Required !!!", result.ErrorsCollection.Members["Address"].Errors.Single().Message);
                Assert.Equal("Required !!!", result.ErrorsCollection.Members["Address"].Errors.Single().ToFormattedMessage());
            }

            [Fact]
            public void Validate_Should_ReturnRequiredError_When_NullRequiredMember_And_RequiredErrorOverriden()
            {
                var validationContext = new ValidationContext(new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c.Member(m => m.Address, m => m.AsModel()))},
                        {typeof(Address), new Specification<Address>(c => c.Valid(m => false).WithMessage("error 2"))}
                    }
                });

                ((ValidationOptions)validationContext.ValidationOptions).RequiredError = new Error("Required 1");

                var result = validationContext.Validate(new User {Address = null}, o => o.SetRequiredError("Required 2"));

                Assert.Equal(0, result.ErrorsCollection.Errors.Count);
                Assert.Equal(1, result.ErrorsCollection.Members.Count);
                Assert.Equal("Required 2", result.ErrorsCollection.Members["Address"].Errors.Single().Message);
            }
        }

        public class Cloning
        {
            public static IEnumerable<object[]> Create_Should_AddValues_Data()
            {
                var userSpecification = new Specification<User>(c => c);
                var addressSpecification = new Specification<Address>(c => c);

                yield return new object[]
                {
                    "ToEmpty", userSpecification, addressSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o),
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification(userSpecification)
                        .AddSpecification(addressSpecification)
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
                    "AddEmpty", userSpecification, addressSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification(userSpecification)
                        .AddSpecification(addressSpecification)
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
                    "AddValues", userSpecification, addressSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification(userSpecification)
                        .AddTranslation("t1", new Dictionary<string, string>
                        {
                            {"a", "A"},
                            {"a1", "A1"},
                            {"a2", "A2"}
                        })),
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification(addressSpecification)
                        .AddTranslation("t2", new Dictionary<string, string>
                        {
                            {"b", "B"}
                        }))
                };

                yield return new object[]
                {
                    "OverrideValues", userSpecification, addressSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification<User>(c => c)
                        .AddSpecification(addressSpecification)
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
                        .AddSpecification(userSpecification)
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
                    "Duplicates", userSpecification, addressSpecification,
                    new Func<IValidationContextOptions, IValidationContextOptions>(o => o
                        .AddSpecification(userSpecification)
                        .AddSpecification(addressSpecification)
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
                        .AddSpecification(userSpecification)
                        .AddSpecification(addressSpecification)
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
            public static void Create_Should_AddValues(string debugName, Specification<User> userSpecification, Specification<Address> addressSpecification, Func<IValidationContextOptions, IValidationContextOptions> original, Func<IValidationContextOptions, IValidationContextOptions> clone)
            {
                Assert.NotNull(debugName);

                var originalValidationContext = new ValidationContextFactory().Create(original);

                var clonedValidationContext = originalValidationContext.Clone(clone);

                Assert.Equal(2, clonedValidationContext.Types.Count);
                ValidationContextTestsHelper.AssertSpecification(clonedValidationContext, userSpecification);
                ValidationContextTestsHelper.AssertSpecification(clonedValidationContext, addressSpecification);

                Assert.Equal(2, clonedValidationContext.Translations.Keys.Count());
                Assert.Equal(3, clonedValidationContext.Translations["t1"].Count);
                Assert.Equal("A", clonedValidationContext.Translations["t1"]["a"]);
                Assert.Equal("A1", clonedValidationContext.Translations["t1"]["a1"]);
                Assert.Equal("A2", clonedValidationContext.Translations["t1"]["a2"]);
                Assert.Equal(1, clonedValidationContext.Translations["t2"].Count);
                Assert.Equal("B", clonedValidationContext.Translations["t2"]["b"]);
            }

            [Fact]
            public void Clone_Should_ReplaceValues()
            {
                var userSpecification = new Specification<User>(m => m);
                var userSpecification2 = new Specification<User>(m => m);

                var original = new ValidationContextFactory().Create(o => o
                    .AddSpecification(userSpecification)
                    .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                    .AddTranslation("t2", new Dictionary<string, string> {{"b", "B"}})
                    .SetNullRootStrategy(NullRootStrategy.NoErrors)
                    .SetTranslationName("t1")
                    .SetValidationStrategy(ValidationStrategy.FailFast)
                    .SetCollectionForceKey("[]")
                    .SetMaxDepth(15)
                    .SetRequiredError("This is required")
                );

                Assert.Equal(1, original.Types.Count);
                ValidationContextTestsHelper.AssertSpecification(original, userSpecification);

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
                Assert.Equal("This is required", original.ValidationOptions.RequiredError.Message);

                var clone = original.Clone(o => o
                    .AddSpecification(userSpecification2)
                    .AddTranslation("t1", new Dictionary<string, string> {{"a", "AA"}})
                    .AddTranslation("t2", new Dictionary<string, string> {{"b", "BB"}})
                    .SetNullRootStrategy(NullRootStrategy.ArgumentNullException)
                    .SetTranslationDisabled()
                    .SetValidationStrategy(ValidationStrategy.Force)
                    .SetCollectionForceKey("[*]")
                    .SetMaxDepth(20)
                    .SetRequiredError("This is required !!!")
                );

                Assert.NotNull(original);
                Assert.NotNull(clone);
                Assert.NotSame(original, clone);

                Assert.Equal(1, clone.Types.Count);
                ValidationContextTestsHelper.AssertSpecification(clone, userSpecification2);

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
                Assert.Equal("This is required !!!", clone.ValidationOptions.RequiredError.Message);

                Assert.NotSame(original.ValidationOptions, clone.ValidationOptions);
                Assert.NotSame(original.ValidationOptions.RequiredError, clone.ValidationOptions.RequiredError);
            }

            [Fact]
            public void Clone_Should_ReturnExactCopy()
            {
                var userSpecification = new Specification<User>(m => m);

                var original = new ValidationContextFactory().Create(o => o
                    .AddSpecification(userSpecification)
                    .AddTranslation("t1", new Dictionary<string, string> {{"a", "A"}})
                    .SetNullRootStrategy(NullRootStrategy.NoErrors)
                    .SetTranslationName("t1")
                    .SetValidationStrategy(ValidationStrategy.FailFast)
                    .SetCollectionForceKey("[]")
                    .SetMaxDepth(15)
                    .SetRequiredError("This is required")
                );

                var clone = original.Clone();

                Assert.NotNull(original);
                Assert.NotNull(clone);
                Assert.NotSame(original, clone);

                void CheckValidationContextVariables(IValidationContext validationContext)
                {
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
                    Assert.Equal("This is required", validationContext.ValidationOptions.RequiredError.Message);
                }

                CheckValidationContextVariables(original);
                CheckValidationContextVariables(clone);
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