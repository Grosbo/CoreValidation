using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CoreValidation.Errors;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Commands
{
    public class MemberScopeTests
    {
        public class TryGetErrors
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_PassStrategy(ValidationStrategy validationStrategy)
            {
                var model = new MemberClass
                {
                    Inner = new InnerClass()
                };

                var executed = new int[3];

                Predicate<InnerClass> isValid1 = c =>
                {
                    Assert.Same(c, model.Inner);
                    executed[0]++;

                    return false;
                };

                Predicate<InnerClass> isValid2 = c =>
                {
                    Assert.Same(c, model.Inner);
                    executed[1]++;

                    return false;
                };

                Predicate<InnerClass> isValid3 = c =>
                {
                    Assert.Same(c, model.Inner);
                    executed[2]++;

                    return false;
                };

                Expression<Func<MemberClass, InnerClass>> selector = m => m.Inner;

                var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

                var memberScope = new MemberScope<MemberClass, InnerClass>(propertyInfo, m => m
                    .Valid(isValid1)
                    .Valid(isValid2)
                    .Valid(isValid3)
                );

                memberScope.TryGetErrors(model, new ExecutionContextStub(), validationStrategy, 0, out _);

                if (validationStrategy == ValidationStrategy.Complete)
                {
                    Assert.True(executed.All(i => i == 1));
                }
                else if (validationStrategy == ValidationStrategy.FailFast)
                {
                    Assert.Equal(1, executed.ElementAt(0));
                    Assert.Equal(0, executed.ElementAt(1));
                    Assert.Equal(0, executed.ElementAt(2));
                }
                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.True(executed.All(i => i == 0));
                }
            }

            [Fact]
            public void Should_PassValue()
            {
                var model = new MemberClass
                {
                    Inner = new InnerClass()
                };

                var executed = 0;

                Predicate<InnerClass> isValid = c =>
                {
                    Assert.Same(c, model.Inner);
                    executed++;

                    return true;
                };

                Expression<Func<MemberClass, InnerClass>> selector = m => m.Inner;

                var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

                var memberScope = new MemberScope<MemberClass, InnerClass>(propertyInfo, m => m
                    .Valid(isValid)
                );

                memberScope.TryGetErrors(model, new ExecutionContextStub(), ValidationStrategy.Complete, 0, out _);

                Assert.Equal(1, executed);
            }

            [Fact]
            public void Should_ReturnDefaultError_IfInvalid()
            {
                var model = new MemberClass
                {
                    Inner = new InnerClass()
                };

                Expression<Func<MemberClass, InnerClass>> selector = m => m.Inner;

                var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

                var memberScope = new MemberScope<MemberClass, InnerClass>(propertyInfo, m => m
                    .Valid(c => false)
                );

                var errors = memberScope.TryGetErrors(model, new ExecutionContextStub
                {
                    DefaultError = new Error("default error")
                }, ValidationStrategy.Complete, 0, out var errorsCollection);

                Assert.True(errors);
                Assert.Single(errorsCollection.Errors);
                Assert.Equal("default error", errorsCollection.Errors.Single().Message);
            }

            [Fact]
            public void Should_ReturnNoErrors_IfValid()
            {
                var model = new MemberClass
                {
                    Inner = new InnerClass()
                };

                Expression<Func<MemberClass, InnerClass>> selector = m => m.Inner;

                var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

                var memberScope = new MemberScope<MemberClass, InnerClass>(propertyInfo, m => m
                    .Valid(c => true)
                );

                var errors = memberScope.TryGetErrors(model, new ExecutionContextStub(), ValidationStrategy.Complete, 0, out var errorsCollection);

                Assert.False(errors);
                Assert.True(errorsCollection.IsEmpty);
            }

            [Fact]
            public void Should_ReturnNoErrors_IfValid_And_RuleSingleErrorSet()
            {
                var model = new MemberClass
                {
                    Inner = new InnerClass()
                };

                Expression<Func<MemberClass, InnerClass>> selector = m => m.Inner;

                var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

                var memberScope = new MemberScope<MemberClass, InnerClass>(propertyInfo, m => m
                    .Valid(c => true)
                );

                memberScope.RuleSingleError = new Error("single error");

                var errors = memberScope.TryGetErrors(model, new ExecutionContextStub(), ValidationStrategy.Complete, 0, out var errorsCollection);

                Assert.False(errors);
                Assert.True(errorsCollection.IsEmpty);
            }

            [Fact]
            public void Should_ReturnRuleSingleError_IfValid()
            {
                var model = new MemberClass
                {
                    Inner = new InnerClass()
                };

                Expression<Func<MemberClass, InnerClass>> selector = m => m.Inner;

                var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

                var memberScope = new MemberScope<MemberClass, InnerClass>(propertyInfo, m => m
                    .Valid(c => false)
                );

                memberScope.RuleSingleError = new Error("single error");

                var errors = memberScope.TryGetErrors(model, new ExecutionContextStub(), ValidationStrategy.Complete, 0, out var errorsCollection);

                Assert.True(errors);
                Assert.Single(errorsCollection.Errors);
                Assert.Equal("single error", errorsCollection.Errors.Single().Message);
            }

            [Fact]
            public void Should_ThrowException_When_NullExecutionContext()
            {
                Expression<Func<MemberClass, string>> selector = m => m.Member;

                var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

                var modelScope = new MemberScope<MemberClass, string>(propertyInfo, m => m);

                Assert.Throws<ArgumentNullException>(() => { modelScope.TryGetErrors(new MemberClass(), null, ValidationStrategy.Complete, 0, out _); });
            }
        }

        public class InsertErrors
        {
            [Fact]
            public void Should_InsertErrors()
            {
                Expression<Func<MemberClass, InnerClass>> selector = m => m.Inner;

                var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

                var memberScope = new MemberScope<MemberClass, InnerClass>(propertyInfo, m => m
                    .Valid(c => false).WithMessage("error1")
                );

                var target = new ErrorsCollection();

                var scopeErrors = new ErrorsCollection();
                scopeErrors.AddError(new Error("error1"));

                memberScope.InsertErrors(target, scopeErrors);

                Assert.Empty(target.Errors);

                Assert.Equal(1, target.Members.Count);

                Assert.Equal("error1", target.Members["Inner"].Errors.Single().Message);
                Assert.Empty(target.Members["Inner"].Members);
            }

            [Fact]
            public void Should_ThrowException_When_NullScopeErrors()
            {
                Expression<Func<MemberClass, InnerClass>> selector = m => m.Inner;

                var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

                var memberScope = new MemberScope<MemberClass, InnerClass>(propertyInfo, m => m
                    .Valid(c => false)
                );

                Assert.Throws<ArgumentNullException>(() => { memberScope.InsertErrors(new ErrorsCollection(), null); });
            }

            [Fact]
            public void Should_ThrowException_When_NullTargetCollection()
            {
                Expression<Func<MemberClass, InnerClass>> selector = m => m.Inner;

                var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

                var memberScope = new MemberScope<MemberClass, InnerClass>(propertyInfo, m => m
                    .Valid(c => false)
                );

                Assert.Throws<ArgumentNullException>(() => { memberScope.InsertErrors(null, new ErrorsCollection()); });
            }
        }

        public class MemberClass
        {
            public string Member { get; set; }
            public InnerClass Inner { get; set; }
        }

        public class InnerClass
        {
            public string InnerMember { get; set; }
        }

        [Fact]
        public void Should_MemberValidator_BeSet()
        {
            Expression<Func<MemberClass, string>> selector = m => m.Member;

            var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

            Predicate<string> isValid = c => true;

            var modelScope = new MemberScope<MemberClass, string>(propertyInfo, m => m
                .Valid(isValid)
            );

            Assert.NotNull(modelScope.MemberValidator);
            Assert.IsType<ValidRule<string>>(modelScope.MemberValidator.Rules.Single());

            var validRule = (ValidRule<string>)modelScope.MemberValidator.Rules.Single();

            Assert.Same(isValid, validRule.IsValid);
        }

        [Fact]
        public void Should_Name_MatchCommandInBuilder()
        {
            Expression<Func<MemberClass, string>> selector = m => m.Member;

            var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

            var modelScope = new MemberScope<MemberClass, string>(propertyInfo, m => m);

            Assert.Equal("Member", modelScope.Name);
        }

        [Fact]
        public void Should_PropetyInfo_BeSet()
        {
            Expression<Func<MemberClass, string>> selector = m => m.Member;

            var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

            var modelScope = new MemberScope<MemberClass, string>(propertyInfo, m => m);

            Assert.Same(propertyInfo, modelScope.MemberPropertyInfo);
        }

        [Fact]
        public void Should_RuleSingleError_BeNullAfterInit()
        {
            Expression<Func<MemberClass, string>> selector = m => m.Member;

            var propertyInfo = (PropertyInfo)((MemberExpression)selector.Body).Member;

            var modelScope = new MemberScope<MemberClass, string>(propertyInfo, m => m);

            Assert.Null(modelScope.RuleSingleError);
        }
    }
}