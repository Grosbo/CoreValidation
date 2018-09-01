using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Options;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

// ReSharper disable CoVariantArrayConversion
// ReSharper disable ObjectCreationAsStatement

namespace CoreValidation.UnitTests.Options
{
    public class OptionsServiceTests
    {
        public static IEnumerable<object[]> GetInvalidValidationOptions()
        {
            yield return new[] {GetVerifiedValidationOptions.ExampleNegativeMaxDepth};
            yield return new[] {GetVerifiedValidationOptions.ExampleNullCollectionForceKey};
            yield return new[] {GetVerifiedValidationOptions.ExampleNullRequiredError};
        }

        private class User
        {
        }

        private class Address
        {
        }


        public class VerifyTranslationName
        {
            public static IEnumerable<object[]> VerifyTranslationName_Should_ThrowException_When_TranslationNotFound_Data()
            {
                yield return new object[] {new Translation[] { }};

                yield return new object[]
                {
                    new[]
                    {
                        new Translation("some_name_1", new Dictionary<string, string>()),
                        new Translation("some_name_2", new Dictionary<string, string>()),
                        new Translation("some_name_3", new Dictionary<string, string>())
                    }
                };
            }

            [Theory]
            [MemberData(nameof(VerifyTranslationName_Should_ThrowException_When_TranslationNotFound_Data))]
            public void VerifyTranslationName_Should_ThrowException_When_TranslationNotFound(Translation[] translations)
            {
                Assert.Throws<TranslationNotFoundException>(() => { OptionsService.VerifyTranslationName(translations, "some_other_value"); });
            }

            [Fact]
            public void VerifyTranslationName_Should_ReturnNull_When_NullDictionaryName()
            {
                var result = OptionsService.VerifyTranslationName(Array.Empty<Translation>(), null);

                Assert.Null(result);
            }

            [Fact]
            public void VerifyTranslationName_Should_ThrowException_When_NullTranslations()
            {
                Assert.Throws<ArgumentNullException>(() => { OptionsService.VerifyTranslationName(null, "some_value"); });
            }

            [Fact]
            public void VerifyTranslationName_Should_VerifyPositive()
            {
                var result = OptionsService.VerifyTranslationName(new[] {new Translation("name", new Dictionary<string, string> {{"a", "b"}})}, "name");

                Assert.Equal("name", result);
            }
        }


        public class GetVerifiedSpecificationsDictionary
        {
            public static IEnumerable<object[]> GetVerifiedSpecificationsDictionary_Should_ThrowException_When_DictionaryContainsNullValue_Data()
            {
                yield return new object[]
                {
                    new Dictionary<Type, object>
                    {
                        {typeof(User), null}
                    }
                };

                yield return new object[]
                {
                    new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c)},
                        {typeof(Address), null}
                    }
                };
            }

            [Theory]
            [MemberData(nameof(GetVerifiedSpecificationsDictionary_Should_ThrowException_When_DictionaryContainsNullValue_Data))]
            public void GetVerifiedSpecificationsDictionary_Should_ThrowException_When_DictionaryContainsNullValue(IReadOnlyDictionary<Type, object> specificationsDictionary)
            {
                Assert.Throws<InvalidOperationException>(() => { OptionsService.GetVerifiedSpecificationsDictionary(specificationsDictionary); });
            }

            public static IEnumerable<object[]> GetVerifiedSpecificationsDictionary_Should_ThrowException_When_InvalidSpecificationTypeForKeyType_Data()
            {
                yield return new object[]
                {
                    new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<Address>(c => c)}
                    }
                };

                yield return new object[]
                {
                    new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c)},
                        {typeof(Address), new Specification<User>(c => c)}
                    }
                };
            }

            [Theory]
            [MemberData(nameof(GetVerifiedSpecificationsDictionary_Should_ThrowException_When_InvalidSpecificationTypeForKeyType_Data))]
            public void GetVerifiedSpecificationsDictionary_Should_ThrowException_When_InvalidSpecificationTypeForKeyType(IReadOnlyDictionary<Type, object> specificationsDictionary)
            {
                Assert.Throws<InvalidSpecificationTypeException>(() => { OptionsService.GetVerifiedSpecificationsDictionary(specificationsDictionary); });
            }

            [Fact]
            public void GetVerifiedSpecificationsDictionary_Should_ThrowException_When_NullDictionary()
            {
                Assert.Throws<ArgumentNullException>(() => { OptionsService.GetVerifiedSpecificationsDictionary(null); });
            }

            [Fact]
            public void GetVerifiedSpecificationsDictionary_Should_ThrowException_When_NullSpecificationAsValue()
            {
                var specificationsDictionary = new Dictionary<Type, object>
                {
                    {typeof(User), new Specification<User>(c => c)},
                    {typeof(Address), null}
                };

                Assert.Throws<InvalidOperationException>(() => { OptionsService.GetVerifiedSpecificationsDictionary(specificationsDictionary); });
            }


            [Fact]
            public void GetVerifiedSpecificationsDictionary_Should_VerifyPositive()
            {
                var specificationsDictionary = new Dictionary<Type, object>
                {
                    {typeof(User), new Specification<User>(c => c)},
                    {typeof(Address), new Specification<Address>(c => c)}
                };

                var result = OptionsService.GetVerifiedSpecificationsDictionary(specificationsDictionary);

                Assert.Equal(2, result.Count);

                Assert.True(result.ContainsKey(typeof(User)));
                Assert.True(result[typeof(User)] == specificationsDictionary[typeof(User)]);

                Assert.True(result.ContainsKey(typeof(Address)));
                Assert.True(result[typeof(Address)] == specificationsDictionary[typeof(Address)]);
            }
        }

        public class GetVerifiedTranslations
        {
            public static IEnumerable<object[]> GetVerifiedTranslations_Should_VerifyPositiveTranslations_Data()
            {
                yield return new object[]
                {
                    new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1"},
                            {"key2", "value2"},
                            {"key3", "value3"}
                        }),
                        new Translation("t2", new Dictionary<string, string>
                        {
                            {"key4", "value4"},
                            {"key5", "value5"},
                            {"key6", "value6"}
                        }),
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "VALUE1"},
                            {"key2", "VALUE2"}
                        }),
                        new Translation("t2", new Dictionary<string, string>
                        {
                            {"key6", "VALUE6"}
                        }),
                        new Translation("t3", new Dictionary<string, string>
                        {
                            {"key3", "value3"}
                        })
                    },
                    new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "VALUE1"},
                            {"key2", "VALUE2"},
                            {"key3", "value3"}
                        }),
                        new Translation("t2", new Dictionary<string, string>
                        {
                            {"key4", "value4"},
                            {"key5", "value5"},
                            {"key6", "VALUE6"}
                        }),
                        new Translation("t3", new Dictionary<string, string>
                        {
                            {"key3", "value3"}
                        })
                    }
                };

                yield return new object[]
                {
                    new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1"},
                            {"key2", "value2"},
                            {"key3", "value3"}
                        }),
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1_"},
                            {"key2", "value2_"},
                            {"key3", "value3_"}
                        }),
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1__"},
                            {"key2", "value2__"},
                            {"key3", "value3__"}
                        })
                    },
                    new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1__"},
                            {"key2", "value2__"},
                            {"key3", "value3__"}
                        })
                    }
                };

                yield return new object[]
                {
                    new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1"}
                        }),
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1_"},
                            {"key2", "value2_"}
                        }),
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1__"},
                            {"key2", "value2__"},
                            {"key3", "value3__"}
                        }),
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key4", "value4__"}
                        })
                    },
                    new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1__"},
                            {"key2", "value2__"},
                            {"key3", "value3__"},
                            {"key4", "value4__"}
                        })
                    }
                };
            }

            [Theory]
            [MemberData(nameof(GetVerifiedTranslations_Should_VerifyPositiveTranslations_Data))]
            public void GetVerifiedTranslations_Should_VerifyPositiveTranslations(IReadOnlyCollection<Translation> translations, IReadOnlyCollection<Translation> expectedTranslations)
            {
                var results = OptionsService.GetVerifiedTranslations(translations);

                Assert.Equal(expectedTranslations.Count, results.Count);

                for (var i = 0; i < expectedTranslations.Count; ++i)
                {
                    var result = results.ElementAt(i);
                    var expected = expectedTranslations.ElementAt(i);

                    Assert.Equal(expected.Name, result.Name);

                    foreach (var expectedKey in expected.Dictionary.Keys)
                    {
                        Assert.Equal(expected.Dictionary[expectedKey], result.Dictionary[expectedKey]);
                    }
                }
            }


            [Fact]
            public void GetVerifiedTranslations_Should_ThrowException_When_NullCollection()
            {
                Assert.Throws<ArgumentNullException>(() => { OptionsService.GetVerifiedTranslations(null); });
            }

            [Fact]
            public void GetVerifiedTranslations_Should_ThrowException_When_NullInCollection()
            {
                var translations = new[]
                {
                    new Translation("test1", new Dictionary<string, string>()),
                    null,
                    new Translation("test2", new Dictionary<string, string>())
                };

                Assert.Throws<InvalidOperationException>(() => { OptionsService.GetVerifiedTranslations(translations); });
            }
        }


        public class GetVerifiedValidationOptions
        {
            public static ValidationOptions ExampleNullCollectionForceKey = new ValidationOptions
            {
                NullRootStrategy = NullRootStrategy.ArgumentNullException,
                ValidationStrategy = ValidationStrategy.FailFast,
                TranslationName = "asd",
                RequiredError = new Error("Required"),
                CollectionForceKey = null,
                MaxDepth = 10
            };

            public static ValidationOptions ExampleNegativeMaxDepth = new ValidationOptions
            {
                NullRootStrategy = NullRootStrategy.ArgumentNullException,
                ValidationStrategy = ValidationStrategy.FailFast,
                TranslationName = "asd",
                RequiredError = new Error("Required"),
                CollectionForceKey = "*",
                MaxDepth = -1
            };

            public static ValidationOptions ExampleNullRequiredError = new ValidationOptions
            {
                NullRootStrategy = NullRootStrategy.ArgumentNullException,
                ValidationStrategy = ValidationStrategy.FailFast,
                TranslationName = "asd",
                RequiredError = null,
                CollectionForceKey = "*",
                MaxDepth = 10
            };


            public static IEnumerable<object[]> GetVerifiedValidationOptions_Should_VerifyPositive_Data()
            {
                yield return new object[]
                {
                    new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.ArgumentNullException,
                        ValidationStrategy = ValidationStrategy.Complete,
                        TranslationName = "translationName",
                        CollectionForceKey = "*",
                        MaxDepth = 15,
                        RequiredError = new Error("required!")
                    }
                };

                yield return new object[]
                {
                    new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.NoErrors,
                        ValidationStrategy = ValidationStrategy.FailFast,
                        TranslationName = null,
                        CollectionForceKey = "[]",
                        MaxDepth = 0,
                        RequiredError = new Error("required {arg}!", new IMessageArg[] {new MessageArg("arg", "value")})
                    }
                };

                yield return new object[]
                {
                    new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.NoErrors,
                        ValidationStrategy = ValidationStrategy.FailFast,
                        TranslationName = "translationName",
                        CollectionForceKey = " ",
                        MaxDepth = 10,
                        RequiredError = new Error("!")
                    }
                };
            }

            [Theory]
            [MemberData(nameof(GetVerifiedValidationOptions_Should_VerifyPositive_Data))]
            public void GetVerifiedValidationOptions_Should_VerifyPositive(ValidationOptions validationOptions)
            {
                var result = OptionsService.GetVerifiedValidationOptions(validationOptions);

                Assert.NotSame(validationOptions, result);
                Assert.Equal(validationOptions.NullRootStrategy, result.NullRootStrategy);
                Assert.Equal(validationOptions.ValidationStrategy, result.ValidationStrategy);
                Assert.Equal(validationOptions.TranslationName, result.TranslationName);
                Assert.Equal(validationOptions.CollectionForceKey, result.CollectionForceKey);
                Assert.Equal(validationOptions.RequiredError.FormattedMessage, result.RequiredError.FormattedMessage);
                Assert.Equal(validationOptions.MaxDepth, result.MaxDepth);
            }

            [Fact]
            public void GetVerifiedValidationOptions_Should_ThrowException_When_NegativeMaxDepth()
            {
                Assert.Throws<InvalidOperationException>(() => { OptionsService.GetVerifiedValidationOptions(ExampleNegativeMaxDepth); });
            }


            [Fact]
            public void GetVerifiedValidationOptions_Should_ThrowException_When_NullArgument()
            {
                Assert.Throws<ArgumentNullException>(() => OptionsService.GetVerifiedValidationOptions(null));
            }

            [Fact]
            public void GetVerifiedValidationOptions_Should_ThrowException_When_NullCollectionForceKey()
            {
                Assert.Throws<InvalidOperationException>(() => { OptionsService.GetVerifiedValidationOptions(ExampleNullCollectionForceKey); });
            }


            [Fact]
            public void GetVerifiedValidationOptions_Should_ThrowException_When_NullRequiredError()
            {
                Assert.Throws<InvalidOperationException>(() => { OptionsService.GetVerifiedValidationOptions(ExampleNullRequiredError); });
            }
        }


        public class GetMerged
        {
            public static IEnumerable<object[]> GetMerged_Should_ThrowException_When_NullOptions_Data()
            {
                yield return new object[]
                {
                    new ValidationContextOptions(),
                    null
                };

                yield return new object[]
                {
                    null,
                    new ValidationContextOptions()
                };

                yield return new object[]
                {
                    null,
                    null
                };
            }

            [Theory]
            [MemberData(nameof(GetMerged_Should_ThrowException_When_NullOptions_Data))]
            public void GetMerged_Should_ThrowException_When_NullOptions(IValidationContextOptions baseOptions, IValidationContextOptions newOptions)
            {
                Assert.Throws<ArgumentNullException>(() => { OptionsService.GetMerged(baseOptions, newOptions); });
            }


            public static IEnumerable<object[]> GetMerged_Should_ThrowException_When_InvalidValidationOptions_Data()
            {
                var invalidValidationOptions = GetInvalidValidationOptions();

                foreach (var options in invalidValidationOptions)
                {
                    yield return new[] {options.Single(), true};
                    yield return new[] {options.Single(), false};
                }
            }

            [Theory]
            [MemberData(nameof(GetMerged_Should_ThrowException_When_InvalidValidationOptions_Data))]
            public void GetMerged_Should_ThrowException_When_InvalidValidationOptions(ValidationOptions validationOptions, bool inBaseOptions)
            {
                var baseOptions = new ValidationContextOptions
                {
                    Translations = new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1__"},
                            {"key2", "value2__"},
                            {"key3", "value3__"}
                        })
                    },
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c)}
                    },
                    ValidationOptions = new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.ArgumentNullException,
                        TranslationName = "t1",
                        ValidationStrategy = ValidationStrategy.FailFast,
                        CollectionForceKey = "*",
                        MaxDepth = 10,
                        RequiredError = new Error("Required")
                    }
                };

                var newOptions = new ValidationContextOptions
                {
                    Translations = new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key2", "value2___"},
                            {"key3", "value3___"},
                            {"key4", "value4___"}
                        }),
                        new Translation("t2", new Dictionary<string, string>
                        {
                            {"key1", "v1"},
                            {"key2", "v2"}
                        })
                    },
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c)},
                        {typeof(Address), new Specification<Address>(c => c)}
                    },
                    ValidationOptions = new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.NoErrors,
                        TranslationName = "t2",
                        ValidationStrategy = ValidationStrategy.Force,
                        CollectionForceKey = "[]",
                        MaxDepth = 5,
                        RequiredError = new Error("Ultimately required")
                    }
                };

                if (inBaseOptions)
                {
                    baseOptions.ValidationOptions = validationOptions;
                }
                else
                {
                    newOptions.ValidationOptions = validationOptions;
                }

                Assert.Throws<InvalidOperationException>(() => { OptionsService.GetMerged(baseOptions, newOptions); });
            }

            [Fact]
            public void GetMerged_Should_GetMerged_FullExample()
            {
                var baseOptions = new ValidationContextOptions
                {
                    Translations = new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1__"},
                            {"key2", "value2__"},
                            {"key3", "value3__"}
                        })
                    },
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c)}
                    },
                    ValidationOptions = new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.ArgumentNullException,
                        TranslationName = "t1",
                        ValidationStrategy = ValidationStrategy.FailFast,
                        CollectionForceKey = "*",
                        MaxDepth = 10,
                        RequiredError = new Error("Required")
                    }
                };

                var newOptions = new ValidationContextOptions
                {
                    Translations = new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key2", "value2___"},
                            {"key3", "value3___"},
                            {"key4", "value4___"}
                        }),
                        new Translation("t2", new Dictionary<string, string>
                        {
                            {"key1", "v1"},
                            {"key2", "v2"}
                        })
                    },
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c)},
                        {typeof(Address), new Specification<Address>(c => c)}
                    },
                    ValidationOptions = new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.NoErrors,
                        TranslationName = "t2",
                        ValidationStrategy = ValidationStrategy.Force,
                        CollectionForceKey = "[]",
                        MaxDepth = 5,
                        RequiredError = new Error("Ultimately required")
                    }
                };

                var result = OptionsService.GetMerged(baseOptions, newOptions);

                Assert.NotNull(result);

                Assert.Equal(2, result.Specifications.Count);

                Assert.Equal(newOptions.Specifications[typeof(User)], result.Specifications[typeof(User)]);
                Assert.Equal(newOptions.Specifications[typeof(Address)], result.Specifications[typeof(Address)]);

                var t1 = result.Translations.First(t => t.Name == "t1");
                Assert.Equal("value1__", t1.Dictionary["key1"]);
                Assert.Equal("value2___", t1.Dictionary["key2"]);
                Assert.Equal("value3___", t1.Dictionary["key3"]);
                Assert.Equal("value4___", t1.Dictionary["key4"]);

                var t2 = result.Translations.First(t => t.Name == "t2");
                Assert.Equal("v1", t2.Dictionary["key1"]);
                Assert.Equal("v2", t2.Dictionary["key2"]);

                Assert.Equal(NullRootStrategy.NoErrors, result.ValidationOptions.NullRootStrategy);
                Assert.Equal("t2", result.ValidationOptions.TranslationName);
                Assert.Equal(ValidationStrategy.Force, result.ValidationOptions.ValidationStrategy);
                Assert.Equal("[]", result.ValidationOptions.CollectionForceKey);
                Assert.Equal(5, result.ValidationOptions.MaxDepth);
                Assert.Equal("Ultimately required", result.ValidationOptions.RequiredError.Message);
                Assert.Equal("Ultimately required", result.ValidationOptions.RequiredError.FormattedMessage);
            }

            [Fact]
            public void GetMerged_Should_GetMerged_InvertedFullExample()
            {
                var baseOptions = new ValidationContextOptions
                {
                    Translations = new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key2", "value2___"},
                            {"key3", "value3___"},
                            {"key4", "value4___"}
                        }),
                        new Translation("t2", new Dictionary<string, string>
                        {
                            {"key1", "v1"},
                            {"key2", "v2"}
                        })
                    },
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c)},
                        {typeof(Address), new Specification<Address>(c => c)}
                    },
                    ValidationOptions = new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.NoErrors,
                        TranslationName = "t2",
                        ValidationStrategy = ValidationStrategy.Force,
                        CollectionForceKey = "[]",
                        MaxDepth = 5,
                        RequiredError = new Error("Ultimately required")
                    }
                };

                var newOptions = new ValidationContextOptions
                {
                    Translations = new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1__"},
                            {"key2", "value2__"},
                            {"key3", "value3__"}
                        })
                    },
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c)}
                    },
                    ValidationOptions = new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.ArgumentNullException,
                        TranslationName = "t1",
                        ValidationStrategy = ValidationStrategy.FailFast,
                        CollectionForceKey = "*",
                        MaxDepth = 10,
                        RequiredError = new Error("Required")
                    }
                };

                var result = OptionsService.GetMerged(baseOptions, newOptions);

                Assert.NotNull(result);

                Assert.Equal(2, result.Specifications.Count);

                Assert.Equal(newOptions.Specifications[typeof(User)], result.Specifications[typeof(User)]);
                Assert.Equal(baseOptions.Specifications[typeof(Address)], result.Specifications[typeof(Address)]);

                var t1 = result.Translations.First(t => t.Name == "t1");
                Assert.Equal("value1__", t1.Dictionary["key1"]);
                Assert.Equal("value2__", t1.Dictionary["key2"]);
                Assert.Equal("value3__", t1.Dictionary["key3"]);
                Assert.Equal("value4___", t1.Dictionary["key4"]);

                var t2 = result.Translations.First(t => t.Name == "t2");
                Assert.Equal("v1", t2.Dictionary["key1"]);
                Assert.Equal("v2", t2.Dictionary["key2"]);

                Assert.Equal(NullRootStrategy.ArgumentNullException, result.ValidationOptions.NullRootStrategy);
                Assert.Equal("t1", result.ValidationOptions.TranslationName);
                Assert.Equal(ValidationStrategy.FailFast, result.ValidationOptions.ValidationStrategy);
            }

            [Fact]
            public void GetMerged_Should_GetMerged_When_TranslationExistOnlyInBased()
            {
                var baseOptions = new ValidationContextOptions
                {
                    Translations = new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key1", "value1__"},
                            {"key2", "value2__"},
                            {"key3", "value3__"}
                        }),
                        new Translation("t2", new Dictionary<string, string>
                        {
                            {"key1", "v1"},
                            {"key2", "v2"}
                        })
                    },
                    Specifications = new Dictionary<Type, object>
                    {
                        {typeof(User), new Specification<User>(c => c)}
                    },
                    ValidationOptions = new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.NoErrors,
                        TranslationName = "t1",
                        ValidationStrategy = ValidationStrategy.Force,
                        CollectionForceKey = "*",
                        MaxDepth = 10,
                        RequiredError = new Error("Required")
                    }
                };

                var newOptions = new ValidationContextOptions
                {
                    Translations = new[]
                    {
                        new Translation("t1", new Dictionary<string, string>
                        {
                            {"key2", "value2___"},
                            {"key3", "value3___"},
                            {"key4", "value4___"}
                        })
                    },
                    ValidationOptions = new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.NoErrors,
                        TranslationName = "t2",
                        ValidationStrategy = ValidationStrategy.Force,
                        CollectionForceKey = "*",
                        MaxDepth = 10,
                        RequiredError = new Error("Required")
                    }
                };

                var result = OptionsService.GetMerged(baseOptions, newOptions);

                Assert.NotNull(result);

                Assert.Equal(1, result.Specifications.Count);
                Assert.Equal(baseOptions.Specifications[typeof(User)], result.Specifications[typeof(User)]);

                var t1 = result.Translations.First(t => t.Name == "t1");
                Assert.Equal("value1__", t1.Dictionary["key1"]);
                Assert.Equal("value2___", t1.Dictionary["key2"]);
                Assert.Equal("value3___", t1.Dictionary["key3"]);
                Assert.Equal("value4___", t1.Dictionary["key4"]);

                Assert.Equal(NullRootStrategy.NoErrors, result.ValidationOptions.NullRootStrategy);
                Assert.Equal("t2", result.ValidationOptions.TranslationName);
                Assert.Equal(ValidationStrategy.Force, result.ValidationOptions.ValidationStrategy);
                Assert.Equal("*", result.ValidationOptions.CollectionForceKey);
                Assert.Equal(10, result.ValidationOptions.MaxDepth);
                Assert.Equal("Required", result.ValidationOptions.RequiredError.Message);
                Assert.Equal("Required", result.ValidationOptions.RequiredError.FormattedMessage);
            }
        }


        public class GetVerifiedValidationContextOptions
        {
            [Theory]
            [MemberData(nameof(GetInvalidValidationOptions), MemberType = typeof(OptionsServiceTests))]
            public void GetVerifiedValidationContextOptions_Should_ThrowException_When_InvalidValidationOptions(ValidationOptions validationOptions)
            {
                var specifications = new Dictionary<Type, object>
                {
                    {typeof(User), new Specification<User>(c => c)},
                    {typeof(Address), new Specification<Address>(c => c)}
                };

                var translations = new[]
                {
                    new Translation("t1", new Dictionary<string, string>
                    {
                        {"key1", "value1"}
                    }),
                    new Translation("t1", new Dictionary<string, string>
                    {
                        {"key1", "value1_"},
                        {"key2", "value2_"}
                    }),
                    new Translation("t1", new Dictionary<string, string>
                    {
                        {"key1", "value1__"},
                        {"key2", "value2__"},
                        {"key3", "value3__"}
                    }),
                    new Translation("t1", new Dictionary<string, string>
                    {
                        {"key4", "value4__"}
                    }),
                    new Translation("t2", new Dictionary<string, string>
                    {
                        {"key5", "value5__"}
                    })
                };

                var options = new ValidationContextOptions
                {
                    Specifications = specifications,
                    Translations = translations,
                    ValidationOptions = validationOptions
                };

                Assert.Throws<InvalidOperationException>(() => OptionsService.GetVerifiedValidationContextOptions(options));
            }

            [Fact]
            public void GetVerifiedValidationContextOptions_Should_ThrowException_When_DefaultTranslationNameDoesntExist()
            {
                var options = new ValidationContextOptions
                {
                    Specifications = new Dictionary<Type, object>(),
                    Translations = new List<Translation>(),
                    ValidationOptions = new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.ArgumentNullException,
                        ValidationStrategy = ValidationStrategy.FailFast,
                        TranslationName = "non_existing",
                        CollectionForceKey = "*",
                        MaxDepth = 10,
                        RequiredError = new Error("Required")
                    }
                };

                Assert.Throws<TranslationNotFoundException>(() => OptionsService.GetVerifiedValidationContextOptions(options));
            }

            [Fact]
            public void GetVerifiedValidationContextOptions_Should_ThrowException_When_NullArgument()
            {
                Assert.Throws<ArgumentNullException>(() => OptionsService.GetVerifiedValidationContextOptions(null));
            }

            [Fact]
            public void GetVerifiedValidationContextOptions_Should_Verify()
            {
                var specifications = new Dictionary<Type, object>
                {
                    {typeof(User), new Specification<User>(c => c)},
                    {typeof(Address), new Specification<Address>(c => c)}
                };

                var translations = new[]
                {
                    new Translation("t1", new Dictionary<string, string>
                    {
                        {"key1", "value1"}
                    }),
                    new Translation("t1", new Dictionary<string, string>
                    {
                        {"key1", "value1_"},
                        {"key2", "value2_"}
                    }),
                    new Translation("t1", new Dictionary<string, string>
                    {
                        {"key1", "value1__"},
                        {"key2", "value2__"},
                        {"key3", "value3__"}
                    }),
                    new Translation("t1", new Dictionary<string, string>
                    {
                        {"key4", "value4__"}
                    }),
                    new Translation("t2", new Dictionary<string, string>
                    {
                        {"key5", "value5__"}
                    })
                };

                var options = new ValidationContextOptions
                {
                    Specifications = specifications,
                    Translations = translations,
                    ValidationOptions = new ValidationOptions
                    {
                        NullRootStrategy = NullRootStrategy.ArgumentNullException,
                        ValidationStrategy = ValidationStrategy.FailFast,
                        TranslationName = "t1",
                        CollectionForceKey = "*",
                        MaxDepth = 10,
                        RequiredError = new Error("Required")
                    }
                };

                var result = OptionsService.GetVerifiedValidationContextOptions(options);

                Assert.NotNull(result);

                Assert.Equal(specifications.Count, result.Specifications.Count);

                Assert.Equal(specifications.ElementAt(0).Key, result.Specifications.ElementAt(0).Key);
                Assert.Equal(specifications.ElementAt(0).Value, result.Specifications.ElementAt(0).Value);
                Assert.Equal(specifications.ElementAt(1).Key, result.Specifications.ElementAt(1).Key);
                Assert.Equal(specifications.ElementAt(1).Value, result.Specifications.ElementAt(1).Value);

                var t1 = result.Translations.First(t => t.Name == "t1");
                Assert.Equal("value1__", t1.Dictionary["key1"]);
                Assert.Equal("value2__", t1.Dictionary["key2"]);
                Assert.Equal("value3__", t1.Dictionary["key3"]);
                Assert.Equal("value4__", t1.Dictionary["key4"]);

                var t2 = result.Translations.First(t => t.Name == "t2");
                Assert.Equal("value5__", t2.Dictionary["key5"]);

                Assert.Equal(ValidationStrategy.FailFast, result.ValidationOptions.ValidationStrategy);
                Assert.Equal("t1", result.ValidationOptions.TranslationName);
                Assert.Equal(NullRootStrategy.ArgumentNullException, result.ValidationOptions.NullRootStrategy);
            }
        }
    }
}