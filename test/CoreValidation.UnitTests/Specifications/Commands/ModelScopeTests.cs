using System;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Commands
{
    public class ModelScopeTests
    {
        public class TryGetErrors
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_PassStrategy(ValidationStrategy validationStrategy)
            {
                var model = new MemberClass();

                var executed = 0;

                var modelScope = new ModelScope<MemberClass>(c =>
                {
                    Assert.Same(c, model);
                    executed = 1;

                    return true;
                });

                modelScope.TryGetErrors(model, new ExecutionContextStub(), validationStrategy, 0, out _);

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, executed);
            }

            [Fact]
            public void Should_PassValue()
            {
                var model = new MemberClass();

                var executed = 0;

                Predicate<MemberClass> isValid = c =>
                {
                    Assert.Same(c, model);
                    executed++;

                    return true;
                };

                var modelScope = new ModelScope<MemberClass>(isValid);

                modelScope.TryGetErrors(model, new ExecutionContextStub(), ValidationStrategy.Complete, 0, out _);

                Assert.Equal(1, executed);
            }

            [Fact]
            public void Should_ReturnDefaultError_IfInvalid()
            {
                var modelScope = new ModelScope<MemberClass>(c => false);

                var errors = modelScope.TryGetErrors(new MemberClass(), new ExecutionContextStub
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
                var modelScope = new ModelScope<MemberClass>(c => true);

                var errors = modelScope.TryGetErrors(new MemberClass(), new ExecutionContextStub(), ValidationStrategy.Complete, 0, out var errorsCollection);

                Assert.False(errors);
                Assert.True(errorsCollection.IsEmpty);
            }

            [Fact]
            public void Should_ReturnNoErrors_IfValid_And_RuleSingleErrorSet()
            {
                var modelScope = new ModelScope<MemberClass>(c => true);
                modelScope.RuleSingleError = new Error("single error");

                var errors = modelScope.TryGetErrors(new MemberClass(), new ExecutionContextStub(), ValidationStrategy.Complete, 0, out var errorsCollection);

                Assert.False(errors);
                Assert.True(errorsCollection.IsEmpty);
            }

            [Fact]
            public void Should_ReturnRuleSingleError_IfValid()
            {
                var modelScope = new ModelScope<MemberClass>(c => false);
                modelScope.RuleSingleError = new Error("single error");

                var errors = modelScope.TryGetErrors(new MemberClass(), new ExecutionContextStub(), ValidationStrategy.Complete, 0, out var errorsCollection);

                Assert.True(errors);
                Assert.Single(errorsCollection.Errors);
                Assert.Equal("single error", errorsCollection.Errors.Single().Message);
            }

            [Fact]
            public void Should_ThrowException_When_NullExecutionContext()
            {
                var modelScope = new ModelScope<MemberClass>(c => true);

                Assert.Throws<ArgumentNullException>(() => { modelScope.TryGetErrors(new MemberClass(), null, ValidationStrategy.Complete, 0, out _); });
            }
        }

        public class InsertErrors
        {
            [Fact]
            public void Should_InsertErrors()
            {
                var target = new ErrorsCollection();

                var scopeErrors = new ErrorsCollection();
                scopeErrors.AddError(new Error("error1"));

                var modelScope = new ModelScope<MemberClass>(c => true);

                modelScope.InsertErrors(target, scopeErrors);

                Assert.Equal("error1", target.Errors.Single().Message);
                Assert.Empty(target.Members);
            }

            [Fact]
            public void Should_ThrowException_When_NullScopeErrors()
            {
                var modelScope = new ModelScope<MemberClass>(c => true);

                Assert.Throws<ArgumentNullException>(() => { modelScope.InsertErrors(new ErrorsCollection(), null); });
            }

            [Fact]
            public void Should_ThrowException_When_NullTargetCollection()
            {
                var modelScope = new ModelScope<MemberClass>(c => true);

                Assert.Throws<ArgumentNullException>(() => { modelScope.InsertErrors(null, new ErrorsCollection()); });
            }
        }

        public class MemberClass
        {
        }

        [Fact]
        public void Should_Name_MatchCommandInBuilder()
        {
            var modelScope = new ModelScope<MemberClass>(c => true);

            Assert.Equal("Valid", modelScope.Name);
        }

        [Fact]
        public void Should_Rule_BeSet()
        {
            Predicate<MemberClass> predicate = c => true;

            var modelScope = new ModelScope<MemberClass>(predicate);

            Assert.NotNull(modelScope.Rule);
            Assert.IsType<AsRelativeRule<MemberClass>>(modelScope.Rule);
            Assert.Same(predicate, modelScope.Rule.IsValid);
            Assert.Null(modelScope.Rule.Error);
        }

        [Fact]
        public void Should_RuleSingleError_BeNullAfterInit()
        {
            var modelScope = new ModelScope<MemberClass>(c => true);

            Assert.Null(modelScope.RuleSingleError);
        }
    }
}