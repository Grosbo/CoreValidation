﻿using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules
{
    public class BoolRulesTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void True_Should_CollectError(bool memberValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.True(),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Bool.True);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void False_Should_CollectError(bool memberValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.False(),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Bool.False);
        }
    }
}