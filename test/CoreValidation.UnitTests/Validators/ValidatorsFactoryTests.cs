using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Validators;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Validators
{
    public class ValidatorsFactoryTests
    {
        private class User
        {
        }

        private class Address
        {
        }

        [Fact]
        public void Should_InitializeEmpty()
        {
            var validatorsFactoryMock = new Mock<ISpecificationsRepository>();

            var validatorsFactory = new ValidatorsFactory(validatorsFactoryMock.Object);

            Assert.Empty(validatorsFactory.Keys);
        }

        [Fact]
        public void Should_InitSpecification()
        {
            var validatorsFactoryMock = new Mock<ISpecificationsRepository>();

            Specification<User> userSpecification = m => m.SetSingleError("user");

            var userSpecificationExecuted = false;

            validatorsFactoryMock
                .Setup(r => r.Get<User>())
                .Returns(() =>
                {
                    userSpecificationExecuted = true;

                    return userSpecification;
                });

            Specification<Address> addressSpecification = m => m.SetSingleError("address");

            var addressSpecificationExecuted = false;

            validatorsFactoryMock
                .Setup(r => r.Get<Address>())
                .Returns(() =>
                {
                    addressSpecificationExecuted = true;

                    return addressSpecification;
                });

            var validatorsFactory = new ValidatorsFactory(validatorsFactoryMock.Object);

            var validator = validatorsFactory.GetOrInit<User>();

            Assert.NotNull(validator);
            Assert.True(userSpecificationExecuted);
            Assert.False(addressSpecificationExecuted);

            Assert.Equal(typeof(User).FullName, validatorsFactory.Keys.Single());

            Assert.Equal("user", validator.SingleError.Message);
        }

        [Fact]
        public void Should_InitSpecification_OnlyOnce()
        {
            var validatorsFactoryMock = new Mock<ISpecificationsRepository>();

            Specification<User> userSpecification = m => m.SetSingleError("user");

            var userSpecificationExecuted = 0;

            validatorsFactoryMock
                .Setup(r => r.Get<User>())
                .Returns(() =>
                {
                    userSpecificationExecuted++;

                    return userSpecification;
                });

            var validatorsFactory = new ValidatorsFactory(validatorsFactoryMock.Object);

            validatorsFactory.GetOrInit<User>();
            validatorsFactory.GetOrInit<User>();
            var validator = validatorsFactory.GetOrInit<User>();

            Assert.NotNull(validator);
            Assert.Equal(1, userSpecificationExecuted);

            Assert.Equal(typeof(User).FullName, validatorsFactory.Keys.Single());

            Assert.Equal("user", validator.SingleError.Message);
        }

        [Fact]
        public void Should_InitSpecification_WithKey()
        {
            var validatorsFactoryMock = new Mock<ISpecificationsRepository>();

            Specification<User> userSpecification = m => m.SetSingleError("user");

            var userSpecificationExecuted = false;

            validatorsFactoryMock
                .Setup(r => r.Get<User>())
                .Returns(() =>
                {
                    userSpecificationExecuted = true;

                    return userSpecification;
                });

            Specification<Address> addressSpecification = m => m.SetSingleError("address");

            var addressSpecificationExecuted = false;

            validatorsFactoryMock
                .Setup(r => r.Get<Address>())
                .Returns(() =>
                {
                    addressSpecificationExecuted = true;

                    return addressSpecification;
                });

            var validatorsFactory = new ValidatorsFactory(validatorsFactoryMock.Object);

            var validator = validatorsFactory.GetOrInit<User>(key: "key");

            Assert.NotNull(validator);
            Assert.True(userSpecificationExecuted);
            Assert.False(addressSpecificationExecuted);

            Assert.Equal("key", validatorsFactory.Keys.Single());

            Assert.Equal("user", validator.SingleError.Message);
        }

        [Fact]
        public void Should_InitSpecification_WithValidator()
        {
            var validatorsFactoryMock = new Mock<ISpecificationsRepository>();

            Specification<User> userSpecification = m => m.SetSingleError("user");

            var userSpecificationExecuted = false;

            validatorsFactoryMock
                .Setup(r => r.Get<User>())
                .Returns(() =>
                {
                    userSpecificationExecuted = true;

                    return userSpecification;
                });

            Specification<Address> addressSpecification = m => m.SetSingleError("address");

            var addressSpecificationExecuted = false;

            validatorsFactoryMock
                .Setup(r => r.Get<Address>())
                .Returns(() =>
                {
                    addressSpecificationExecuted = true;

                    return addressSpecification;
                });

            var validatorsFactory = new ValidatorsFactory(validatorsFactoryMock.Object);

            Specification<User> userDefinedSpecification = m => m.SetSingleError("user_defined");

            var validator = validatorsFactory.GetOrInit(userDefinedSpecification);

            Assert.NotNull(validator);
            Assert.False(userSpecificationExecuted);
            Assert.False(addressSpecificationExecuted);

            Assert.Equal(typeof(User).FullName, validatorsFactory.Keys.Single());

            Assert.Equal("user_defined", validator.SingleError.Message);
        }

        [Fact]
        public void Should_InitSpecification_WithValidator_And_WithKey()
        {
            var validatorsFactoryMock = new Mock<ISpecificationsRepository>();

            Specification<User> userSpecification = m => m.SetSingleError("user");

            var userSpecificationExecuted = false;

            validatorsFactoryMock
                .Setup(r => r.Get<User>())
                .Returns(() =>
                {
                    userSpecificationExecuted = true;

                    return userSpecification;
                });

            Specification<Address> addressSpecification = m => m.SetSingleError("address");

            var addressSpecificationExecuted = false;

            validatorsFactoryMock
                .Setup(r => r.Get<Address>())
                .Returns(() =>
                {
                    addressSpecificationExecuted = true;

                    return addressSpecification;
                });

            var validatorsFactory = new ValidatorsFactory(validatorsFactoryMock.Object);

            Specification<User> userDefinedSpecification = m => m.SetSingleError("user_defined");

            var validator = validatorsFactory.GetOrInit(userDefinedSpecification, "key");

            Assert.NotNull(validator);
            Assert.False(userSpecificationExecuted);
            Assert.False(addressSpecificationExecuted);

            Assert.Equal("key", validatorsFactory.Keys.Single());

            Assert.Equal("user_defined", validator.SingleError.Message);
        }

        [Fact]
        public void Should_ThrowException_If_GettingNonExistingSpecification_ByKey()
        {
            var validatorsFactoryMock = new Mock<ISpecificationsRepository>();

            validatorsFactoryMock
                .Setup(r => r.Get<Address>())
                .Returns(n => n);

            var validatorsFactory = new ValidatorsFactory(validatorsFactoryMock.Object);

            Assert.Throws<KeyNotFoundException>(() => { validatorsFactory.Get<Address>("random"); });
        }

        [Fact]
        public void Should_ThrowException_If_GettingNonExistingSpecification_ByType()
        {
            var validatorsFactoryMock = new Mock<ISpecificationsRepository>();

            validatorsFactoryMock
                .Setup(r => r.Get<User>())
                .Returns(n => n);

            var validatorsFactory = new ValidatorsFactory(validatorsFactoryMock.Object);

            Assert.Throws<KeyNotFoundException>(() => { validatorsFactory.Get<Address>(); });
        }

        [Fact]
        public void Should_ThrowException_If_InitializeWithNullSpecificationsRepository()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new ValidatorsFactory(null);
            });
        }
    }
}