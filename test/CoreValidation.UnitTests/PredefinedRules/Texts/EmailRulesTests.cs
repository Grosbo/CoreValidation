﻿using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Texts
{
    public class EmailRulesTests
    {
        [Theory]
        [InlineData(@"prettyandsimple@example.com", true)]
        [InlineData(@"very.common@example.com", true)]
        [InlineData(@"disposable.style.email.with+symbol@example.com", true)]
        [InlineData(@"other.email-with-dash@example.com", true)]
        [InlineData(@"fully-qualified-domain@example.com.", true)]
        [InlineData(@"user.name+tag+sorting@example.com", true)]
        [InlineData(@"x@example.com", true)]
        [InlineData(@"example-indeed@strange-example.com", true)]
        [InlineData(@"admin@mailserver1", true)]
        [InlineData(@"email@123.123.123.123", true)]
        [InlineData(@"email@[123.123.123.123]", true)]
        [InlineData(@"1234567890@example.com", true)]
        [InlineData(@"#!$%&'*+-/=?^_`{}|~@example.org", true)]
        [InlineData(@"""()<>[]:,;@\\\""!#$%&'-/=?^_`{}| ~.a""@example.org", true)]
        [InlineData(@"Abc.example.com", false)]
        [InlineData(@"A@b@c@example.com", false)]
        [InlineData(@"a""b(c)d,e:f;g<h>i[j\k]l@example.com", false)]
        [InlineData(@"just""not""right@example.com", false)]
        [InlineData(@"this is""not\allowed@example.com", false)]
        [InlineData(@"this\ still\""not\\allowed@example.com", false)]
        [InlineData(@"Duy", false)]
        [InlineData(@" email@example.com", false)]
        [InlineData(@"email@example.com ", false)]
        public void Email_Should_CollectError(string value, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.Email(),
                value,
                expectedIsValid,
                Phrases.Keys.Texts.Email);
        }
    }
}