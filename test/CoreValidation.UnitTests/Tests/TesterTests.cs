using System;
using CoreValidation.Errors.Args;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.Tests
{
    public class TesterTests
    {
        public class TestSingleMemberRule
        {
            [Fact]
            public void Should_Test_When_Invalid()
            {
                Tester.TestSingleMemberRule(
                    m => m.False(),
                    true,
                    false);
            }

            [Fact]
            public void Should_Test_When_Invalid_WithArgs()
            {
                Tester.TestSingleMemberRule(
                    m => m.Valid(v => false, "error", new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 1)}),
                    true,
                    false,
                    "error",
                    new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 1)});
            }

            [Fact]
            public void Should_Test_When_Invalid_WithMessage()
            {
                Tester.TestSingleMemberRule(
                    m => m.False().WithMessage("error"),
                    true,
                    false,
                    "error");
            }

            [Fact]
            public void Should_Test_When_InvalidNullable()
            {
                Tester.TestSingleMemberRule(
                    m => m.AsNullable(n => n.False()),
                    (bool?)true,
                    false);
            }


            [Fact]
            public void Should_Test_When_Valid()
            {
                Tester.TestSingleMemberRule(
                    m => m.True(),
                    true,
                    true);
            }

            [Fact]
            public void Should_Test_When_ValidNullable()
            {
                Tester.TestSingleMemberRule(
                    m => m.AsNullable(n => n.True()),
                    (bool?)true,
                    true);
            }

            [Fact]
            public void Should_ThrowException_When_ExpectedInvalid_And_Valid()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.True(),
                        true,
                        false);
                });
            }

            [Fact]
            public void Should_ThrowException_When_ExpectedValid_And_Invalid()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.True(),
                        false,
                        true);
                });
            }

            [Fact]
            public void Should_ThrowException_When_Invalid_WithArgs_And_InvalidCount()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.Valid(v => false, "error", new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 1)}),
                        true,
                        false,
                        "error",
                        new IMessageArg[] {new TextArg("n1", "v1")});
                });
            }

            [Fact]
            public void Should_ThrowException_When_Invalid_WithArgs_And_InvalidDoubleValue()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.Valid(v => false, "error", new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 0.000001)}),
                        true,
                        false,
                        "error",
                        new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 0.000002)});
                });
            }

            [Fact]
            public void Should_ThrowException_When_Invalid_WithArgs_And_InvalidName()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.Valid(v => false, "error", new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 1)}),
                        true,
                        false,
                        "error",
                        new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("xxx", 1)});
                });
            }

            [Fact]
            public void Should_ThrowException_When_Invalid_WithArgs_And_InvalidType()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.Valid(v => false, "error", new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 1)}),
                        true,
                        false,
                        "error",
                        new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 1d)});
                });
            }

            [Fact]
            public void Should_ThrowException_When_Invalid_WithArgs_And_InvalidValue()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.Valid(v => false, "error", new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 1)}),
                        true,
                        false,
                        "error",
                        new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 2)});
                });
            }


            [Fact]
            public void Should_ThrowException_When_InvalidMessage()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.False().WithMessage("error123"),
                        true,
                        false,
                        "error");
                });
            }

            [Fact]
            public void Should_ThrowException_When_InvalidRule()
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.AsModel(),
                        new MemberClass(),
                        false);
                });
            }


            [Fact]
            public void Should_ThrowException_When_ManyErrors()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.AsNullable(n => n.True().True()),
                        (bool?)false,
                        false);
                });
            }

            [Fact]
            public void Should_ThrowException_When_NoArgs_And_ArgsAreExpected()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.False().WithMessage("error"),
                        true,
                        false,
                        "error",
                        new[] {new TextArg("name", "value")});
                });
            }

            [Fact]
            public void Should_ThrowException_When_NoMessage_And_MessageIsExpected()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.False().WithMessage("error123"),
                        true,
                        false,
                        "error");
                });
            }

            [Fact]
            public void Should_ThrowException_When_WithArgs_And_ArgsAreNotExpected()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestSingleMemberRule(
                        m => m.Valid(v => false, "error", new IMessageArg[] {new TextArg("n1", "v1"), NumberArg.Create("n2", 1)}),
                        true,
                        false,
                        "error");
                });
            }
        }

        public class TestMemberRuleException
        {
            [Fact]
            public void Should_ReturnException_When_Thrown()
            {
                var memberException = new MemberException();

                var exception = Tester.TestMemberRuleException<object>(
                    m => throw memberException,
                    typeof(MemberException));

                Assert.Same(memberException, exception);
            }

            [Fact]
            public void Should_ReturnException_When_ThrownAssignable()
            {
                var memberException = new MemberException();

                var exception = Tester.TestMemberRuleException<object>(
                    m => throw memberException,
                    typeof(Exception));

                Assert.Same(memberException, exception);
            }

            [Fact]
            public void Should_ThrowException_When_NothingThrown()
            {
                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestMemberRuleException<object>(
                        m => m,
                        typeof(Exception));
                });
            }

            [Fact]
            public void Should_ThrowException_When_ThrownNotAssignable()
            {
                var memberException = new Exception();

                Assert.Throws<TesterException>(() =>
                {
                    Tester.TestMemberRuleException<object>(
                        m => throw memberException,
                        typeof(MemberException));
                });
            }
        }

        public class MemberClass
        {
        }

        public class MemberException : Exception
        {
        }
    }
}