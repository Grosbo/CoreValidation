using System;
using System.Collections.Generic;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedTranslations
{
    public class AddPolishTranslationTests
    {
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

            var validationContext = ValidationContext.Factory.Create(o => o.IncludeInEnglishTranslation(additionalPhrases));

            PredefinedTranslationsHelper.AssertInclude(validationContext, nameof(Phrases.English), additionalPhrases);
        }

        [Fact]
        public void Should_ThrowException_When_Include_And_NullPhrases()
        {
            Assert.Throws<ArgumentNullException>(() => { ValidationContext.Factory.Create(o => o.IncludeInEnglishTranslation(null)); });
        }
    }
}