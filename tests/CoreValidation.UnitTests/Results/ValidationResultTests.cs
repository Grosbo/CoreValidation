using System;
using CoreValidation.Errors;
using CoreValidation.Results;
using CoreValidation.Translations;
using CoreValidation.UnitTests.Errors;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace CoreValidation.UnitTests.Results
{
    public class ValidationResultTests
    {
        [Fact]
        public void Should_Merge()
        {
            var nested11 = new ErrorsCollection();

            nested11.AddError("arg11", new Error("val111"));
            nested11.AddError("arg11", new Error("val112"));

            var nested12 = new ErrorsCollection();

            nested12.AddError("arg12", new Error("val121"));
            nested12.AddError("arg12", new Error("val122"));

            var nested1 = new ErrorsCollection();

            nested1.AddError("arg1", new Error("val11"));
            nested1.AddError("arg1", new Error("val12"));

            nested1.AddError("arg1", nested11);
            nested1.AddError("arg1", nested12);

            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("foo", nested1);

            var validationResult = new ValidationResult<object>(Guid.Empty, new TranslationProxy(e => e.StringifiedMessage, new TranslatorsRepository(Array.Empty<Translation>())), new RulesOptionsStub(), new object(), errorsCollection);

            var anotherNested12 = new ErrorsCollection();

            anotherNested12.AddError("arg12", new Error("val122"));
            anotherNested12.AddError("arg12", new Error("val123"));

            var anotherNested13 = new ErrorsCollection();

            anotherNested13.AddError("arg13", new Error("val131"));
            anotherNested13.AddError("arg13", new Error("val132"));

            var anotherNested1 = new ErrorsCollection();

            anotherNested1.AddError("arg1", new Error("val12"));
            anotherNested1.AddError("arg1", new Error("val13"));
            anotherNested1.AddError("arg1", anotherNested12);
            anotherNested1.AddError("arg1", anotherNested13);

            var another = new ErrorsCollection();

            another.AddError("foo", anotherNested1);

            Assert.False(validationResult.ContainsMergedErrors);
            Assert.Same(errorsCollection, validationResult.ErrorsCollection);

            var mergedResult = validationResult.Merge(another);

            Assert.NotSame(mergedResult, validationResult);
            Assert.NotSame(mergedResult, another);
            Assert.True(mergedResult.ContainsMergedErrors);
            Assert.NotSame(errorsCollection, mergedResult.ErrorsCollection);

            ErrorsCollectionTestsHelpers.ExpectMembers(mergedResult.ErrorsCollection, new[] {"foo"});

            ErrorsCollectionTestsHelpers.ExpectMembers(mergedResult.ErrorsCollection.Members["foo"], new[] {"arg1"});

            ErrorsCollectionTestsHelpers.ExpectErrors(mergedResult.ErrorsCollection.Members["foo"].Members["arg1"], new[] {"val11", "val12", "val13"});
            ErrorsCollectionTestsHelpers.ExpectMembers(mergedResult.ErrorsCollection.Members["foo"].Members["arg1"], new[] {"arg11", "arg12", "arg13"});

            ErrorsCollectionTestsHelpers.ExpectErrors(mergedResult.ErrorsCollection.Members["foo"].Members["arg1"].Members["arg11"], new[] {"val111", "val112"});
            ErrorsCollectionTestsHelpers.ExpectErrors(mergedResult.ErrorsCollection.Members["foo"].Members["arg1"].Members["arg12"], new[] {"val121", "val122", "val123"});
            ErrorsCollectionTestsHelpers.ExpectErrors(mergedResult.ErrorsCollection.Members["foo"].Members["arg1"].Members["arg13"], new[] {"val131", "val132"});
        }

        [Fact]
        public void Should_RecordValidationDate()
        {
            var now = DateTime.UtcNow;

            var result = new ValidationResult<object>(Guid.Empty, new TranslationProxy(e => e.StringifiedMessage, new TranslatorsRepository(Array.Empty<Translation>())), new RulesOptionsStub(), new object(), new ErrorsCollection());

            Assert.True(result.ValidationDate.Subtract(now) < TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void Should_SaveGuid()
        {
            var guid = Guid.NewGuid();

            var result = new ValidationResult<object>(guid, new TranslationProxy(e => e.StringifiedMessage, new TranslatorsRepository(Array.Empty<Translation>())), new RulesOptionsStub(), new object(), new ErrorsCollection());

            Assert.Equal(guid, result.CoreValidatorId);
        }

        [Fact]
        public void Should_SaveModel()
        {
            var model = new object();

            var result = new ValidationResult<object>(Guid.Empty, new TranslationProxy(e => e.StringifiedMessage, new TranslatorsRepository(Array.Empty<Translation>())), new RulesOptionsStub(), model, new ErrorsCollection());

            Assert.Same(model, result.Model);
        }

        [Fact]
        public void Should_SaveModel_When_Null()
        {
            var result = new ValidationResult<object>(Guid.Empty, new TranslationProxy(e => e.StringifiedMessage, new TranslatorsRepository(Array.Empty<Translation>())), new RulesOptionsStub());

            Assert.Null(result.Model);
        }

        [Fact]
        public void Should_ThrowException_When_Merge_And_NullErrorCollection()
        {
            var result = new ValidationResult<object>(Guid.Empty, new TranslationProxy(e => e.StringifiedMessage, new TranslatorsRepository(Array.Empty<Translation>())), new RulesOptionsStub(), new object(), new ErrorsCollection());

            Assert.Throws<ArgumentNullException>(() => { result.Merge(null); });
            Assert.Throws<ArgumentException>(() => { result.Merge(new ErrorsCollection(), null); });
        }


        [Fact]
        public void Should_ThrowException_When_NullRulesOptions()
        {
            Assert.Throws<ArgumentNullException>(() => { new ValidationResult<object>(Guid.Empty, new TranslationProxy(e => e.StringifiedMessage, new TranslatorsRepository(Array.Empty<Translation>())), null, new object(), new ErrorsCollection()); });
        }

        [Fact]
        public void Should_ThrowException_When_NullTranslationProxy()
        {
            Assert.Throws<ArgumentNullException>(() => { new ValidationResult<object>(Guid.Empty, null, new RulesOptionsStub(), new object(), new ErrorsCollection()); });
        }
    }
}