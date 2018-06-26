using System;
using CoreValidation.Errors;
using CoreValidation.Results;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Results
{
    public class IsValidExtensionTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_ReturnIsValid(bool isValid)
        {
            var errorsCollectionMock = new Mock<IErrorsCollection>();
            errorsCollectionMock.Setup(r => r.IsEmpty).Returns(isValid);

            var resultMock = new Mock<IValidationResult<object>>();

            resultMock
                .Setup(r => r.ErrorsCollection)
                .Returns(errorsCollectionMock.Object);

            var result = resultMock.Object;

            Assert.Equal(isValid, result.IsValid());
        }

        [Fact]
        public void Should_ThrowException_If_NullThis()
        {
            Assert.Throws<ArgumentNullException>(() => { (null as IValidationResult<object>).IsValid(); });
        }
    }
}