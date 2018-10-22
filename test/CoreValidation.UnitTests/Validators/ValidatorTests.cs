using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CoreValidation.Specifications.Commands;
using CoreValidation.Validators;
using Xunit;

namespace CoreValidation.UnitTests.Validators
{
    public class ValidatorTests
    {
        public class MemberClass
        {
            public string Member { get; set; }
        }

        [Fact]
        public void Should_AddScope_MultipleRules()
        {
            var validator = new Validator<object>();

            Expression<Func<MemberClass, string>> selector = m => m.Member;

            var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

            var scope1 = new ModelScope<object>(c => true);
            var scope2 = new MemberScope<object, object>(propertyInfo, m => m);
            validator.AddScope(scope1);
            validator.AddScope(scope2);

            Assert.Equal(2, validator.Scopes.Count);
            Assert.Same(scope1, validator.Scopes.ElementAt(0));
            Assert.Same(scope2, validator.Scopes.ElementAt(1));
        }

        [Fact]
        public void Should_AddScope_SingleScope()
        {
            var validator = new Validator<object>();

            var scope = new ModelScope<object>(c => true);
            validator.AddScope(scope);

            Assert.Single(validator.Scopes);
            Assert.Same(scope, validator.Scopes.Single());
        }

        [Fact]
        public void Should_AddScope_ThrowException_When_NullRule()
        {
            var validator = new Validator<object>();

            Assert.Throws<ArgumentNullException>(() => { validator.AddScope(null); });
        }
    }
}