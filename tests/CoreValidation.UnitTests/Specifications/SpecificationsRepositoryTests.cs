using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Validators;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class SpecificationsRepositoryTests
    {
        [Fact]
        public void Should_InitializeEmpty()
        {
            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            var specificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object);

            Assert.Empty(specificationsRepository.Keys);
        }

        [Fact]
        public void Should_InitSpecification()
        {
            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            Validator<User> userValidator = m => m.WithSummaryError("user");

            var userValidatorExecuted = false;

            validatorsRepositoryMock
                .Setup(r => r.Get<User>())
                .Returns(() =>
                {
                    userValidatorExecuted = true;
                    return userValidator;
                });


            Validator<Address> addressValidator = m => m.WithSummaryError("address");

            var addressValidatorExecuted = false;

            validatorsRepositoryMock
                .Setup(r => r.Get<Address>())
                .Returns(() =>
                {
                    addressValidatorExecuted = true;
                    return addressValidator;
                });

            var specificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object);

            var specification = specificationsRepository.GetOrInit<User>();

            Assert.NotNull(specification);
            Assert.True(userValidatorExecuted);
            Assert.False(addressValidatorExecuted);

            Assert.Equal(typeof(User).FullName, specificationsRepository.Keys.Single());

            Assert.Equal("user", specification.SummaryError.Message);
        }

        [Fact]
        public void Should_InitSpecification_WithKey()
        {
            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            Validator<User> userValidator = m => m.WithSummaryError("user");

            var userValidatorExecuted = false;

            validatorsRepositoryMock
                .Setup(r => r.Get<User>())
                .Returns(() =>
                {
                    userValidatorExecuted = true;
                    return userValidator;
                });

            Validator<Address> addressValidator = m => m.WithSummaryError("address");

            var addressValidatorExecuted = false;

            validatorsRepositoryMock
                .Setup(r => r.Get<Address>())
                .Returns(() =>
                {
                    addressValidatorExecuted = true;
                    return addressValidator;
                });

            var specificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object);

            var specification = specificationsRepository.GetOrInit<User>(key: "key");

            Assert.NotNull(specification);
            Assert.True(userValidatorExecuted);
            Assert.False(addressValidatorExecuted);

            Assert.Equal("key", specificationsRepository.Keys.Single());

            Assert.Equal("user", specification.SummaryError.Message);
        }

        [Fact]
        public void Should_InitSpecification_WithValidator()
        {
            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            Validator<User> userValidator = m => m.WithSummaryError("user");

            var userValidatorExecuted = false;

            validatorsRepositoryMock
                .Setup(r => r.Get<User>())
                .Returns(() =>
                {
                    userValidatorExecuted = true;
                    return userValidator;
                });

            Validator<Address> addressValidator = m => m.WithSummaryError("address");

            var addressValidatorExecuted = false;

            validatorsRepositoryMock
                .Setup(r => r.Get<Address>())
                .Returns(() =>
                {
                    addressValidatorExecuted = true;
                    return addressValidator;
                });

            var specificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object);

            Validator<User> userDefinedValidator = m => m.WithSummaryError("user_defined");

            var specification = specificationsRepository.GetOrInit(userDefinedValidator);

            Assert.NotNull(specification);
            Assert.False(userValidatorExecuted);
            Assert.False(addressValidatorExecuted);

            Assert.Equal(typeof(User).FullName, specificationsRepository.Keys.Single());

            Assert.Equal("user_defined", specification.SummaryError.Message);
        }

        [Fact]
        public void Should_InitSpecification_WithValidator_And_WithKey()
        {
            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            Validator<User> userValidator = m => m.WithSummaryError("user");

            var userValidatorExecuted = false;

            validatorsRepositoryMock
                .Setup(r => r.Get<User>())
                .Returns(() =>
                {
                    userValidatorExecuted = true;
                    return userValidator;
                });

            Validator<Address> addressValidator = m => m.WithSummaryError("address");

            var addressValidatorExecuted = false;

            validatorsRepositoryMock
                .Setup(r => r.Get<Address>())
                .Returns(() =>
                {
                    addressValidatorExecuted = true;
                    return addressValidator;
                });

            var specificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object);

            Validator<User> userDefinedValidator = m => m.WithSummaryError("user_defined");

            var specification = specificationsRepository.GetOrInit(userDefinedValidator, "key");

            Assert.NotNull(specification);
            Assert.False(userValidatorExecuted);
            Assert.False(addressValidatorExecuted);

            Assert.Equal("key", specificationsRepository.Keys.Single());

            Assert.Equal("user_defined", specification.SummaryError.Message);
        }

        [Fact]
        public void Should_InitSpecification_OnlyOnce()
        {
            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            Validator<User> userValidator = m => m.WithSummaryError("user");

            var userValidatorExecuted = 0;

            validatorsRepositoryMock
                .Setup(r => r.Get<User>())
                .Returns(() =>
                {
                    userValidatorExecuted++;
                    return userValidator;
                });


            var specificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object);

            specificationsRepository.GetOrInit<User>();
            specificationsRepository.GetOrInit<User>();
            var specification = specificationsRepository.GetOrInit<User>();

            Assert.NotNull(specification);
            Assert.Equal(1, userValidatorExecuted);

            Assert.Equal(typeof(User).FullName, specificationsRepository.Keys.Single());

            Assert.Equal("user", specification.SummaryError.Message);
        }

        [Fact]
        public void Should_ThrowException_If_InitializeWithNullValidatorsRepository()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new SpecificationsRepository(null);
            });
        }

        [Fact]
        public void Should_ThrowException_If_GettingNonExistingSpecification_ByType()
        {
            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            validatorsRepositoryMock
                .Setup(r => r.Get<User>())
                .Returns(n => n);

            var specificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object);

            Assert.Throws<KeyNotFoundException>(() =>
            {
                specificationsRepository.Get<Address>();
            });
        }

        [Fact]
        public void Should_ThrowException_If_GettingNonExistingSpecification_ByKey()
        {
            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            validatorsRepositoryMock
                .Setup(r => r.Get<Address>())
                .Returns(n => n);

            var specificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object);

            Assert.Throws<KeyNotFoundException>(() =>
            {
                specificationsRepository.Get<Address>("random");
            });
        }

        private class User
        {

        }

        private class Address
        {

        }
    }
}