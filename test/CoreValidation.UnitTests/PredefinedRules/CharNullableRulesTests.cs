using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules
{
    public class CharNullableRulesTests
    {
        [Theory]
        [InlineData('a', 'a', true)]
        [InlineData('A', 'a', true)]
        [InlineData('a', 'A', true)]
        [InlineData('A', 'A', true)]
        [InlineData('A', 'b', false)]
        [InlineData('a', 'B', false)]
        [InlineData('a', 'b', false)]
        [InlineData('A', 'B', false)]
        [InlineData('Ż', 'Ż', true)]
        [InlineData('ć', 'Ć', true)]
        [InlineData('Ą', 'ó', false)]
        public void EqualIgnoreCase_Should_CollectError(char model, char value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, char?>();

            builder.EqualIgnoreCase(value);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Char.EqualIgnoreCase);
        }

        [Theory]
        [InlineData('a', 'a', false)]
        [InlineData('A', 'a', false)]
        [InlineData('a', 'A', false)]
        [InlineData('A', 'A', false)]
        [InlineData('A', 'b', true)]
        [InlineData('a', 'B', true)]
        [InlineData('a', 'b', true)]
        [InlineData('A', 'B', true)]
        [InlineData('Ż', 'Ż', false)]
        [InlineData('ć', 'Ć', false)]
        [InlineData('Ą', 'ó', true)]
        public void NotEqualIgnoreCase_Should_CollectError(char model, char value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, char?>();

            builder.NotEqualIgnoreCase(value);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Char.NotEqualIgnoreCase);
        }

        public class MessageTests
        {
            [Fact]
            public void EqualIgnoreCase_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, char?>();

                builder.EqualIgnoreCase('e', "{value} Overriden error message");

                RulesHelper.AssertErrorMessage('c', builder.Rules, "{value} Overriden error message", "e Overriden error message");
            }

            [Fact]
            public void NotEqualIgnoreCase_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, char?>();

                builder.NotEqualIgnoreCase('c', "{value} Overriden error message");

                RulesHelper.AssertErrorMessage('c', builder.Rules, "{value} Overriden error message", "c Overriden error message");
            }
        }
    }
}