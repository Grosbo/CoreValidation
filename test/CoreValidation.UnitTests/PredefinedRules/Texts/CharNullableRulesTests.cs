using CoreValidation.Errors.Args;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Texts
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
        public void EqualIgnoreCase_Should_CollectError(char memberValue, char argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<char?>(
                m => m.EqualIgnoreCase(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Char.EqualIgnoreCase,
                new IMessageArg[]
                {
                    new TextArg("value", argValue)
                });
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
        public void NotEqualIgnoreCase_Should_CollectError(char memberValue, char argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<char?>(
                m => m.NotEqualIgnoreCase(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Char.NotEqualIgnoreCase,
                new IMessageArg[]
                {
                    new TextArg("value", argValue)
                });
        }
    }
}