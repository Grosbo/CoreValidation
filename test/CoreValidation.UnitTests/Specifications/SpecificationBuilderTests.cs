using System;
using System.Linq;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class SpecificationBuilderTests
    {
        public class SetSingleError
        {
            [Fact]
            public void Should_Add_SetSingleErrorCommand_When_SetSingleError()
            {
                var builder = new SpecificationBuilder<object>();

                builder.SetSingleError("message");

                Assert.Single(builder.Commands);
                Assert.IsType<SetSingleErrorCommand>(builder.Commands.Single());

                var command = (SetSingleErrorCommand)builder.Commands.Single();

                Assert.Equal("message", command.Message);
                Assert.Equal("SetSingleError", command.Name);
            }

            [Fact]
            public void Should_ThrowException_When_SetSingleError_With_NullMessage()
            {
                var builder = new SpecificationBuilder<object>();

                Assert.Throws<ArgumentNullException>(() => { builder.SetSingleError(null); });
            }
        }

        public class WithMessage
        {
            [Fact]
            public void Should_Add_WithMessageCommand_When_WithMessage()
            {
                var builder = new SpecificationBuilder<object>();

                builder.WithMessage("message");

                Assert.Single(builder.Commands);
                Assert.IsType<WithMessageCommand>(builder.Commands.Single());

                var command = (WithMessageCommand)builder.Commands.Single();

                Assert.Equal("WithMessage", command.Name);
                Assert.Equal("message", command.Message);
            }

            [Fact]
            public void Should_ThrowException_When_WithMessage_And_NullMessage()
            {
                var builder = new SpecificationBuilder<object>();

                Assert.Throws<ArgumentNullException>(() => { builder.WithMessage(null); });
            }
        }

        public class Valid
        {
            [Fact]
            public void Should_Add_ModelScope_When_Valid()
            {
                var builder = new SpecificationBuilder<MemberClass>();

                Predicate<MemberClass> isValid = c => true;

                builder.Valid(isValid);

                Assert.Single(builder.Commands);
                Assert.IsType<ModelScope<MemberClass>>(builder.Commands.Single());

                var command = (ModelScope<MemberClass>)builder.Commands.Single();

                Assert.Equal("Valid", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.Same(isValid, command.Rule.IsValid);
                Assert.Null(command.Rule.RuleSingleError);
                Assert.Null(command.Rule.Error);
            }

            [Fact]
            public void Should_ThrowException_When_Valid_And_NullPredicate()
            {
                var builder = new SpecificationBuilder<MemberClass>();

                Assert.Throws<ArgumentNullException>(() => { builder.Valid(null); });
            }
        }

        public class Member
        {
            [Fact]
            public void Should_Add_MemberScope_When_Member()
            {
                var builder = new SpecificationBuilder<MemberClass>();

                Predicate<string> isValid = c => true;

                var args = new IMessageArg[] {new MessageArg("test", "test123")};

                builder.Member(m => m.Property, m => m.Valid(isValid, "message", args));

                Assert.Single(builder.Commands);
                Assert.IsType<MemberScope<MemberClass, string>>(builder.Commands.Single());

                var memberScope = (MemberScope<MemberClass, string>)builder.Commands.Single();
                Assert.Null(memberScope.RuleSingleError);

                Assert.Equal("Property", memberScope.MemberPropertyInfo.Name);
                Assert.Equal(typeof(string), memberScope.MemberPropertyInfo.PropertyType);

                Assert.IsType<ValidRule<string>>(memberScope.MemberValidator.Rules.Single());

                var memberRule = (ValidRule<string>)memberScope.MemberValidator.Rules.Single();

                Assert.Same(isValid, memberRule.IsValid);
                Assert.Equal("message", memberRule.Error.Message);
                Assert.Same(args, memberRule.Error.Arguments);
                Assert.Null(memberRule.RuleSingleError);
            }

            [Fact]
            public void Should_AddMemberScope_When_Member_WithoutSpecification()
            {
                var builder = new SpecificationBuilder<MemberClass>();

                builder.Member(m => m.Property);

                Assert.IsType<MemberScope<MemberClass, string>>(builder.Commands.Single());

                var memberScope = (MemberScope<MemberClass, string>)builder.Commands.Single();

                Assert.Null(memberScope.RuleSingleError);

                Assert.Equal("Property", memberScope.MemberPropertyInfo.Name);
                Assert.Equal(typeof(string), memberScope.MemberPropertyInfo.PropertyType);

                Assert.False(memberScope.MemberValidator.IsOptional);
                Assert.Null(memberScope.MemberValidator.RequiredError);
                Assert.Empty(memberScope.MemberValidator.Rules);
            }

            [Fact]
            public void Should_ThrowException_When_Member_And_NullMemberSelector()
            {
                var builder = new SpecificationBuilder<MemberClass>();

                Assert.Throws<ArgumentNullException>(() => { builder.Member<string>(null); });
            }
        }

        public class MemberClass
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public string Property { get; }
        }

        public class CustomCommand : ICommand
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public string Name { get; }
        }

        [Fact]
        public void Should_AddCommand()
        {
            var builder = new SpecificationBuilder<MemberClass>();

            var command = new CustomCommand();

            builder.AddCommand(command);

            Assert.Single(builder.Commands);
            Assert.Same(command, builder.Commands.Single());
        }

        [Fact]
        public void Should_AddCommand_ThrowException_When_NullCommand()
        {
            var builder = new SpecificationBuilder<MemberClass>();

            Assert.Throws<ArgumentNullException>(() => { builder.AddCommand(null); });
        }
    }
}