using System;
using System.Linq;
using CoreValidation.Specifications.Commands;
using CoreValidation.Validators;
using Xunit;

namespace CoreValidation.UnitTests.Validators
{
    public class MemberValidatorTests
    {
        [Fact]
        public void Should_AddRule_MultipleRules()
        {
            var memberValidator = new MemberValidator();

            var rule1 = new ValidRule<int>(c => true);
            var rule2 = new AsRelativeRule<object>(c => true);
            var rule3 = new AsModelRule<object>();

            memberValidator.AddRule(rule1);
            memberValidator.AddRule(rule2);
            memberValidator.AddRule(rule3);

            Assert.Equal(3, memberValidator.Rules.Count);
            Assert.Same(rule1, memberValidator.Rules.ElementAt(0));
            Assert.Same(rule2, memberValidator.Rules.ElementAt(1));
            Assert.Same(rule3, memberValidator.Rules.ElementAt(2));
        }

        [Fact]
        public void Should_AddRule_SingleRule()
        {
            var memberValidator = new MemberValidator();

            var rule = new ValidRule<int>(c => true);
            memberValidator.AddRule(rule);

            Assert.Single(memberValidator.Rules);
            Assert.Same(rule, memberValidator.Rules.Single());
        }

        [Fact]
        public void Should_AddRule_ThrowException_When_NullRule()
        {
            var memberValidator = new MemberValidator();

            Assert.Throws<ArgumentNullException>(() => { memberValidator.AddRule(null); });
        }
    }
}