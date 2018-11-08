using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.UnitTests.Translations
{
    public class TranslatorsRepositoryTests
    {
        public static IEnumerable<object[]> Constructor_Should_ThrowException_When_DuplicateTranslations_Data()
        {
            yield return new object[]
            {
                new[]
                {
                    new Translation("test1", new Dictionary<string, string>()),
                    new Translation("test1", new Dictionary<string, string>())
                }
            };

            yield return new object[]
            {
                new[]
                {
                    new Translation("test1", new Dictionary<string, string>()),
                    new Translation("test2", new Dictionary<string, string>()),
                    new Translation("test1", new Dictionary<string, string>())
                }
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_Should_ThrowException_When_DuplicateTranslations_Data))]
        public void Constructor_Should_ThrowException_When_DuplicateTranslations(IReadOnlyCollection<Translation> translations)
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<DuplicateTranslationException>(() => { new TranslatorsRepository(translations); });
        }

        public static IEnumerable<object[]> TranslationsNames_Should_GroupTranslationNames_Data()
        {
            yield return new object[]
            {
                new Translation[]
                {
                },
                new string[] { }
            };

            yield return new object[]
            {
                new[]
                {
                    new Translation("test1", new Dictionary<string, string>())
                },
                new[] {"test1"}
            };

            yield return new object[]
            {
                new[]
                {
                    new Translation("test1", new Dictionary<string, string>()),
                    new Translation("test2", new Dictionary<string, string>())
                },
                new[] {"test1", "test2"}
            };
        }

        [Theory]
        [MemberData(nameof(TranslationsNames_Should_GroupTranslationNames_Data))]
        public void TranslationsNames_Should_GroupTranslationNames(IReadOnlyCollection<Translation> translations, IReadOnlyCollection<string> expectedTranslationNames)
        {
            var translatorsRepository = new TranslatorsRepository(translations);

            Assert.Equal(expectedTranslationNames.Count(), translatorsRepository.Translations.Count());

            foreach (var expected in expectedTranslationNames)
            {
                Assert.True(translatorsRepository.Translations.ContainsKey(expected));
            }
        }

        [Fact]
        public void Constructor_Should_ThrowException_NullArgumentInConstructor()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new TranslatorsRepository(null));
        }

        [Fact]
        public void Get_Should_GetSelectedTranslator_When_NonExistingPhrases()
        {
            var translations = new[]
            {
                new Translation("test1", new Dictionary<string, string>
                {
                    {"phrase1", "PHRASE_1"},
                    {"phrase2", "PHRASE_2"},
                    {"phrase{0}", "PHRASE_{0}"}
                })
            };

            var translatorsRepository = new TranslatorsRepository(translations);

            var translator = translatorsRepository.Get("test1");

            Assert.Equal("phrase123", translator(new Error("phrase123")));
            Assert.Equal("some_phrase", translator(new Error("some_phrase")));
            Assert.Equal("some_another_phrase_123", translator(new Error("some_another_phrase_{0}", new[] {Arg.Text("0", "123")})));
        }

        [Fact]
        public void Get_Should_ReturnSelectedTranslator_And_Translate_When_ExistingPhrases()
        {
            var translations = new[]
            {
                new Translation("test1", new Dictionary<string, string>
                {
                    {"phrase1", "PHRASE_1"},
                    {"phrase2", "PHRASE_2"},
                    {"phrase{0}", "PHRASE_{0}"}
                }),
                new Translation("test2", new Dictionary<string, string>
                {
                    {"phrase1", "PHRASE=1"},
                    {"phrase2", "PHRASE=2"},
                    {"phrase{0}", "PHRASE={0}"}
                })
            };

            var translatorsRepository = new TranslatorsRepository(translations);

            var translator1 = translatorsRepository.Get("test1");
            var translator2 = translatorsRepository.Get("test2");

            Assert.Equal("PHRASE_1", translator1(new Error("phrase1")));
            Assert.Equal("PHRASE_2", translator1(new Error("phrase2")));
            Assert.Equal("PHRASE_123", translator1(new Error("phrase{0}", new[] {Arg.Text("0", "123")})));

            Assert.Equal("PHRASE=1", translator2(new Error("phrase1")));
            Assert.Equal("PHRASE=2", translator2(new Error("phrase2")));
            Assert.Equal("PHRASE=123", translator2(new Error("phrase{0}", new[] {Arg.Text("0", "123")})));
        }

        [Fact]
        public void Get_Should_ThrowException_When_NullTranslationName()
        {
            var translations = new[]
            {
                new Translation("test1", new Dictionary<string, string>
                {
                    {"phrase1", "PHRASE_1"},
                    {"phrase2", "PHRASE_2"},
                    {"phrase{0}", "PHRASE_{0}"}
                })
            };

            var translatorsRepository = new TranslatorsRepository(translations);

            Assert.Throws<ArgumentNullException>(() => { translatorsRepository.Get(null); });
        }

        [Fact]
        public void Get_Should_ThrowException_When_TranslationNotFound()
        {
            var translations = new[]
            {
                new Translation("test1", new Dictionary<string, string>
                {
                    {"phrase1", "PHRASE_1"},
                    {"phrase2", "PHRASE_2"},
                    {"phrase{0}", "PHRASE_{0}"}
                })
            };

            var translatorsRepository = new TranslatorsRepository(translations);

            var exception = Assert.Throws<TranslationNotFoundException>(() => { translatorsRepository.Get("some_another"); });

            Assert.Equal("some_another", exception.Name);
        }
    }
}