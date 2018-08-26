using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules
{
    public class BoolRulesTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void True_Should_CollectError(bool value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, bool>();

            builder.True();

            RulesHelper.AssertErrorCompilation(value, builder.Rules, expectedIsValid, Phrases.Keys.Bool.True);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void False_Should_CollectError(bool value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, bool>();

            builder.False();

            RulesHelper.AssertErrorCompilation(value, builder.Rules, expectedIsValid, Phrases.Keys.Bool.False);
        }

        public class MessageTests
        {
            [Fact]
            public void False_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, bool>();

                builder.False("Overriden error message");

                RulesHelper.AssertErrorMessage(true, builder.Rules, "Overriden error message", "Overriden error message");
            }

            [Fact]
            public void True_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, bool>();

                builder.True("Overriden error message");

                RulesHelper.AssertErrorMessage(false, builder.Rules, "Overriden error message", "Overriden error message");
            }
        }
    }
}