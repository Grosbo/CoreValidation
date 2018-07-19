using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Validators;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class MemberSpecificationTests
    {
        public static IEnumerable<object[]> Should_SetMessage_Data()
        {
            yield return new object[]
            {
                "message {argKey}", new[] {new MessageArg("argKey", "argValue")}
            };

            yield return new object[]
            {
                "message {argKey1} {argKey2} {argKey3}",
                new[]
                {
                    new MessageArg("argKey1", "argValue1"),
                    new MessageArg("argKey2", "argValue2"),
                    new MessageArg("argKey3", "argValue3")
                }
            };
        }

        [Theory]
        [MemberData(nameof(Should_SetMessage_Data))]
        public void Should_SetSummaryError_When_WithSummaryError(string message, IReadOnlyCollection<IMessageArg> messageArgs)
        {
            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.WithSummaryError(message, messageArgs);

            Assert.NotNull(memberSpecification.SummaryError);

            Assert.Equal(message, memberSpecification.SummaryError.Message);
            Assert.Same(messageArgs, memberSpecification.SummaryError.Arguments);
        }

        public static IEnumerable<object[]> Should_SetMessage_With_NullArguments_Data()
        {
            yield return new object[]
            {
                "message {argKey}"
            };

            yield return new object[]
            {
                "message {argKey1} {argKey2} {argKey3}"
            };
        }

        [Theory]
        [MemberData(nameof(Should_SetMessage_With_NullArguments_Data))]
        public void Should_SetError_With_NullArguments_When_WithSummaryError(string message)
        {
            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.WithSummaryError(message);

            Assert.NotNull(memberSpecification.SummaryError);

            Assert.Equal(message, memberSpecification.SummaryError.Message);
            Assert.Null(memberSpecification.SummaryError.Arguments);
        }

        [Theory]
        [MemberData(nameof(Should_SetMessage_Data))]
        public void Should_SetRequiredErrorMessage(string message, IReadOnlyCollection<IMessageArg> messageArgs)
        {
            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.WithRequiredError(message, messageArgs);

            Assert.NotNull(memberSpecification.RequiredError);

            Assert.Equal(message, memberSpecification.RequiredError.Message);
            Assert.Same(messageArgs, memberSpecification.RequiredError.Arguments);
        }

        [Theory]
        [MemberData(nameof(Should_SetMessage_With_NullArguments_Data))]
        public void Should_SetRequiredErrorMessage_With_NullArguments(string message)
        {
            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.WithRequiredError(message);

            Assert.NotNull(memberSpecification.RequiredError);

            Assert.Equal(message, memberSpecification.RequiredError.Message);
            Assert.Null(memberSpecification.RequiredError.Arguments);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("__            *")]
        public void Should_SetName_When_WithName(string name)
        {
            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.WithName(name);

            Assert.Equal(name, memberSpecification.Name);

            Assert.Null(memberSpecification.SummaryError);
            Assert.False(memberSpecification.IsOptional);
            Assert.Empty(memberSpecification.Rules);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("      ")]
        [InlineData("  \n    ")]
        [InlineData("  \r\n    ")]
        public void Should_ThrowException_When_WithName_And_InvalidName(string name)
        {
            var memberSpecification = new MemberSpecification<object, object>();

            void Rename() => memberSpecification.WithName(name);

            if (name == null)
            {
                Assert.Throws<ArgumentNullException>((Action)Rename);
            }
            else
            {
                Assert.Throws<ArgumentException>((Action)Rename);
            }
        }

        public static IEnumerable<object[]> Should_AddRule_Data()
        {
            yield return new object[] {new ValidRelativeRule<object>(c => true, "message")};
            yield return new object[] {new ValidRule<object>(c => true, "message")};
            yield return new object[] {new ValidModelRule<object>(c => c)};
            yield return new object[] {new ValidModelRule<object>()};
            yield return new object[] {new ValidNullableRule<object, int>(c => c)};
        }

        [Theory]
        [MemberData(nameof(Should_AddRule_Data))]
        public void Should_AddRule(IRule rule)
        {
            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.AddRule(rule);

            Assert.Same(rule, memberSpecification.Rules.Single());
        }

        public static IEnumerable<object[]> Should_AddValidateItemsRule_When_ValidateNestedItems_Data()
        {
            yield return new object[] {new Validator<object>(c => c), true};
            yield return new object[] {new Validator<object>(c => c), false};

            yield return new object[] {null, true};
            yield return new object[] {null, false};
        }

        [Theory]
        [MemberData(nameof(Should_AddValidateItemsRule_When_ValidateNestedItems_Data))]
        public void Should_AddValidateItemsRule_When_ValidateNestedItems(Validator<object> validator, bool isOptional)
        {
            var memberSpecification = new MemberSpecification<object, IEnumerable<object>>();

            memberSpecification.ValidModelsCollection<object, IEnumerable<object>, object>(validator, isOptional);

            Assert.IsType<ValidCollectionRule<object, object>>(memberSpecification.Rules.Single());

            var validateItemsRule = (ValidCollectionRule<object, object>)memberSpecification.Rules.Single();

            Assert.NotNull(validateItemsRule.MemberValidator);

            var processed = validateItemsRule.MemberValidator(new MemberSpecification<object, object>()) as MemberSpecification<object, object>;

            Assert.NotNull(processed);
            Assert.IsType<ValidModelRule<object>>(processed.Rules.Single());
            Assert.Equal(isOptional, processed.IsOptional);

            var validatorRule = (ValidModelRule<object>)processed.Rules.Single();

            Assert.Same(validator, validatorRule.Validator);
        }

        [Fact]
        public void Should_AddRule_MultipleRules()
        {
            var memberSpecification = new MemberSpecification<object, object>();

            var rules = new IRule[]
            {
                new ValidRelativeRule<object>(c => true, "message"),
                new ValidRule<object>(c => true, "message"),
                new ValidModelRule<object>(c => c),
                new ValidModelRule<object>(),
                new ValidNullableRule<object, int>(c => c)
            };

            for (var i = 0; i < rules.Length; ++i)
            {
                memberSpecification.AddRule(rules.ElementAt(i));
            }

            Assert.Equal(rules.Length, memberSpecification.Rules.Count);

            for (var i = 0; i < rules.Length; ++i)
            {
                Assert.Same(rules.ElementAt(i), memberSpecification.Rules.ElementAt(i));
            }
        }

        [Fact]
        public void Should_AddValidateNestedRule_WithoutValidator_When_ValidateNested_And_NullValidator()
        {
            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.ValidModel();

            Assert.IsType<ValidModelRule<object>>(memberSpecification.Rules.Single());

            var validatorRule = (ValidModelRule<object>)memberSpecification.Rules.Single();

            Assert.Null(validatorRule.Validator);
        }

        [Fact]
        public void Should_AddValidateNestedRule_WithValidator_When_ValidateNested()
        {
            Validator<object> validator = c => c;

            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.ValidModel(validator);

            Assert.IsType<ValidModelRule<object>>(memberSpecification.Rules.Single());

            var validatorRule = (ValidModelRule<object>)memberSpecification.Rules.Single();

            Assert.Same(validator, validatorRule.Validator);
        }

        [Fact]
        public void Should_AddValidateNullableRule_When_ValidateNullable()
        {
            var memberSpecification = new MemberSpecification<object, int?>();

            MemberValidator<object, int> collectNullable = c => c;

            memberSpecification.ValidNullable(collectNullable);

            Assert.IsType<ValidNullableRule<object, int>>(memberSpecification.Rules.Single());

            var validatorRule = (ValidNullableRule<object, int>)memberSpecification.Rules.Single();

            Assert.Same(collectNullable, validatorRule.MemberValidator);
        }

        [Fact]
        public void Should_AddValidateRelationRule_When_ValidateRelation()
        {
            Predicate<object> isValid = c => true;

            var memberSpecification = new MemberSpecification<object, object>();

            var args = new[] {new MessageArg("key", "value")};

            memberSpecification.ValidRelative(isValid, "error message", args);

            Assert.IsType<ValidRelativeRule<object>>(memberSpecification.Rules.Single());

            var modelRule = (ValidRelativeRule<object>)memberSpecification.Rules.Single();

            Assert.Same(isValid, modelRule.IsValid);
            Assert.Equal("error message", modelRule.Message);
            Assert.Same(args, modelRule.Arguments);
        }

        [Fact]
        public void Should_AddValidateRule_When_Validate()
        {
            Predicate<object> isValid = c => true;

            var memberSpecification = new MemberSpecification<object, object>();

            var args = new[] {new MessageArg("key", "value")};

            memberSpecification.Valid(isValid, "error message", args);

            Assert.IsType<ValidRule<object>>(memberSpecification.Rules.Single());

            var memberRule = (ValidRule<object>)memberSpecification.Rules.Single();

            Assert.Same(isValid, memberRule.IsValid);
            Assert.Equal("error message", memberRule.Message);
            Assert.Same(args, memberRule.Arguments);
        }

        [Fact]
        public void Should_AddValidateRule_When_Validate_And_NullArgs()
        {
            Predicate<object> isValid = c => true;

            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.Valid(isValid, "error message");

            Assert.IsType<ValidRule<object>>(memberSpecification.Rules.Single());

            var memberRule = (ValidRule<object>)memberSpecification.Rules.Single();

            Assert.Same(isValid, memberRule.IsValid);
            Assert.Equal("error message", memberRule.Message);
            Assert.Null(memberRule.Arguments);
        }

        [Fact]
        public void Should_AddValidateRule_When_Validate_And_NullMessage_And_NullArgs()
        {
            Predicate<object> isValid = c => true;

            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.Valid(isValid);

            Assert.IsType<ValidRule<object>>(memberSpecification.Rules.Single());

            var memberRule = (ValidRule<object>)memberSpecification.Rules.Single();

            Assert.Same(isValid, memberRule.IsValid);
            Assert.Null(memberRule.Message);
            Assert.Null(memberRule.Arguments);
        }

        [Fact]
        public void Should_HaveEmptyInitialValues()
        {
            var memberSpecification = new MemberSpecification<object, object>();

            Assert.False(memberSpecification.IsOptional);
            Assert.Null(memberSpecification.SummaryError);
            Assert.Empty(memberSpecification.Rules);
        }

        [Fact]
        public void Should_SetOptional()
        {
            var memberSpecification = new MemberSpecification<object, object>();

            memberSpecification.Optional();

            Assert.True(memberSpecification.IsOptional);

            Assert.Null(memberSpecification.SummaryError);
            Assert.Empty(memberSpecification.Rules);
        }



        [Fact]
        public void Should_ThrowException_When_AddRule_And_NullArgument()
        {
            var memberSpecification = new MemberSpecification<object, object>();

            Assert.Throws<ArgumentNullException>(() => { memberSpecification.AddRule(null); });
        }

        [Fact]
        public void Should_ThrowException_When_DuplicateOptional()
        {
            var memberSpecification = new MemberSpecification<object, object>();

            Assert.Throws<InvalidOperationException>(() => { memberSpecification.Optional().Optional(); });
        }

        [Fact]
        public void Should_ThrowException_When_DuplicateWithSummaryError()
        {
            var memberSpecification = new MemberSpecification<object, object>();

            Assert.Throws<InvalidOperationException>(() => { memberSpecification.WithSummaryError("test1").WithSummaryError("test2"); });
        }

        [Fact]
        public void Should_ThrowException_When_SetName_MoreThanOnce()
        {
            var memberSpecification = new MemberSpecification<object, object>();

            var memberSpecificationWithName = memberSpecification.WithName("test1");

            Assert.Throws<InvalidOperationException>(() => { memberSpecificationWithName.WithName("test2"); });
        }

        [Fact]
        public void Should_ThrowException_When_Validate_And_NullPredicate()
        {
            var memberSpecification = new MemberSpecification<object, int?>();

            Assert.Throws<ArgumentNullException>(() => { memberSpecification.Valid(null, "test"); });
        }

        [Fact]
        public void Should_ThrowException_When_ValidateNullable_And_NullArgument()
        {
            var memberSpecification = new MemberSpecification<object, int?>();

            Assert.Throws<ArgumentNullException>(() => { memberSpecification.ValidNullable(null); });
        }

        [Fact]
        public void Should_ThrowException_When_WithSummaryError_With_NullMessage()
        {
            var memberSpecification = new MemberSpecification<object, object>();

            Assert.Throws<ArgumentNullException>(() => { memberSpecification.WithSummaryError(null); });
        }
    }
}