using System;
using System.Collections.Generic;
using CoreValidation.PredefinedTranslations;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedTranslations
{
    public class PolishTranslationExtensionsTests
    {
        [Fact]
        public void Should_AddTranslations()
        {
            var validationContext = ValidationContext.Factory.Create(o => o.AddPolishTranslation());

            PredefinedTranslationsHelper.AssertAddTranslations(validationContext, "Polish", new PolishTranslation());
        }

        [Fact]
        public void Should_AddTranslations_And_IncludeAdditionalPhrases()
        {
            var additionalPhrases = new Dictionary<string, string>
            {
                {"Test1", "TEST1"},
                {"Test2", "TEST2"},
                {"Test3", "TEST3"},
                {Phrases.Keys.Texts.Email, "TEST_EMAIL"},
                {Phrases.Keys.Required, "TEST_REQUIRED"}
            };

            var validationContext = ValidationContext.Factory.Create(o => o.AddPolishTranslation(include: additionalPhrases));

            PredefinedTranslationsHelper.AssertInclude(validationContext, nameof(Phrases.Polish), additionalPhrases);
        }

        [Fact]
        public void Should_Include()
        {
            var additionalPhrases = new Dictionary<string, string>
            {
                {"Test1", "TEST1"},
                {"Test2", "TEST2"},
                {"Test3", "TEST3"},
                {Phrases.Keys.Texts.Email, "TEST_EMAIL"},
                {Phrases.Keys.Required, "TEST_REQUIRED"}
            };

            var validationContext = ValidationContext.Factory.Create(o => o.IncludeInPolishTranslation(additionalPhrases));

            PredefinedTranslationsHelper.AssertInclude(validationContext, nameof(Phrases.Polish), additionalPhrases);
        }

        [Fact]
        public void Should_SetAsDefault()
        {
            var validationContext = ValidationContext.Factory.Create(o => o.AddPolishTranslation(true));

            PredefinedTranslationsHelper.AssertSetAsDefault(validationContext, nameof(Phrases.Polish));
        }

        [Fact]
        public void Should_ThrowException_When_Include_And_NullPhrases()
        {
            Assert.Throws<ArgumentNullException>(() => { ValidationContext.Factory.Create(o => o.IncludeInPolishTranslation(null)); });
        }
    }
}