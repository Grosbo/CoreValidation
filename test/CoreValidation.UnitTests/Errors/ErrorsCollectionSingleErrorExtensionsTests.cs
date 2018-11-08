using CoreValidation.Errors;
using Xunit;

namespace CoreValidation.UnitTests.Errors
{
    public class ErrorsCollectionSingleErrorExtensionsTests
    {
        public class ContainsSingleError
        {
            [Fact]
            public void ContainsSingleError_Should_ReturnFalse_When_ManyErrors()
            {
                var errorsCollection = new ErrorsCollection();

                errorsCollection.AddError(new Error("test123"));
                errorsCollection.AddError(new Error("test1234"));
                errorsCollection.AddError(new Error("test12345"));

                Assert.False(errorsCollection.ContainsSingleError());
            }

            [Fact]
            public void ContainsSingleError_Should_ReturnFalse_When_MemberError()
            {
                var errorsCollection = new ErrorsCollection();

                errorsCollection.AddError("member", new Error("test123"));

                Assert.False(errorsCollection.ContainsSingleError());
            }

            [Fact]
            public void ContainsSingleError_Should_ReturnFalse_When_No()
            {
                var errorsCollection = new ErrorsCollection();

                Assert.False(errorsCollection.ContainsSingleError());
            }

            [Fact]
            public void ContainsSingleError_Should_ReturnTrue_When_SingleError()
            {
                var errorsCollection = new ErrorsCollection();

                errorsCollection.AddError(new Error("test123"));

                Assert.True(errorsCollection.ContainsSingleError());
            }
        }

        public class GetSingleError
        {
            [Fact]
            public void GetSingleError_Should_ReturnFalse_When_ManyErrors()
            {
                var errorsCollection = new ErrorsCollection();

                errorsCollection.AddError(new Error("test123"));
                errorsCollection.AddError(new Error("test1234"));
                errorsCollection.AddError(new Error("test12345"));

                Assert.Throws<NoSingleErrorCollectionException>(() => { errorsCollection.GetSingleError(); });
            }

            [Fact]
            public void GetSingleError_Should_ReturnFalse_When_MemberError()
            {
                var errorsCollection = new ErrorsCollection();

                errorsCollection.AddError("member", new Error("test123"));

                Assert.Throws<NoSingleErrorCollectionException>(() => { errorsCollection.GetSingleError(); });
            }

            [Fact]
            public void GetSingleError_Should_ReturnFalse_When_No()
            {
                var errorsCollection = new ErrorsCollection();

                Assert.Throws<NoSingleErrorCollectionException>(() => { errorsCollection.GetSingleError(); });
            }

            [Fact]
            public void GetSingleError_Should_ReturnTrue_When_SingleError()
            {
                var errorsCollection = new ErrorsCollection();

                var error = new Error("test123");

                errorsCollection.AddError(error);

                Assert.Same(error, errorsCollection.GetSingleError());
            }
        }
    }
}