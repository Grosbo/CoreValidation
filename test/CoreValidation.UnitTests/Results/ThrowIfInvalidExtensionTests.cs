using System;
using CoreValidation.Errors;
using CoreValidation.Results;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Results
{
    public class ThrowIfInvalidExtensionTests
    {
        public class Item
        {
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_ThrowIfInvalid(bool isValid)
        {
            var errorsCollectionMock = new Mock<IErrorsCollection>();
            errorsCollectionMock.Setup(r => r.IsEmpty).Returns(isValid);

            var resultMock = new Mock<IValidationResult<Item>>();

            var model = new Item();

            resultMock
                .Setup(r => r.ErrorsCollection)
                .Returns(errorsCollectionMock.Object);

            resultMock
                .Setup(r => r.Model)
                .Returns(model);

            var result = resultMock.Object;

            if (!isValid)
            {
                var thrown = false;

                try
                {
                    result.ThrowIfInvalid();
                }
                catch (InvalidModelException<Item> exception)
                {
                    Assert.Same(model, exception.Model);
                    Assert.Same(result, exception.ValidationResult);
                    Assert.Equal(typeof(Item), exception.Type);

                    var general = exception as InvalidModelException;

                    Assert.Same(model, general.Model);

                    thrown = true;
                }

                Assert.True(thrown);
            }
            else
            {
                result.ThrowIfInvalid();
            }
        }

        [Fact]
        public void Should_ThrowException_If_NullThis()
        {
            Assert.Throws<ArgumentNullException>(() => { (null as IValidationResult<object>).ThrowIfInvalid(); });
        }
    }
}