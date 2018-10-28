using System;
using CoreValidation.Errors;
using CoreValidation.Options;
using CoreValidation.Results;
using CoreValidation.Translations;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Results
{
    public class ThrowResultIfInvalidExtensionTests
    {
        public class Item
        {
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_ThrowResultIfInvalid(bool isValid)
        {
            var errorCollection = new ErrorsCollection();

            if (!isValid)
            {
                errorCollection.AddError(new Error("error"));
            }

            var model = new Item();

            var varlidationResult = new ValidationResult<Item>(Guid.NewGuid(), new Mock<ITranslationProxy>().Object, new Mock<IExecutionOptions>().Object, model, errorCollection);

            if (!isValid)
            {
                var thrown = false;

                try
                {
                    varlidationResult.ThrowResultIfInvalid();
                }
                catch (ValidationResultException<Item> exception)
                {
                    Assert.Same(model, exception.Model);
                    Assert.Same(varlidationResult, exception.ValidationResult);
                    Assert.Equal(typeof(Item), exception.Type);

                    thrown = true;
                }

                Assert.True(thrown);
            }
            else
            {
                varlidationResult.ThrowResultIfInvalid();
            }
        }

        [Fact]
        public void Should_ThrowException_If_NullThis()
        {
            Assert.Throws<ArgumentNullException>(() => { (null as IValidationResult<object>).ThrowResultIfInvalid(); });
        }
    }
}