using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Validators;
using Xunit;

namespace CoreValidation.UnitTests.Validators
{
    public class ValidatorsRepositoryTests
    {
        private class User
        {
        }

        private class Company
        {
        }

        public static IEnumerable<object[]> RegisteredTypes_Data()
        {
            yield return new object[]
            {
                new Dictionary<Type, object>(),

                new Type[]
                {
                }
            };

            yield return new object[]
            {
                new Dictionary<Type, object>
                {
                    {typeof(User), new Validator<User>(c => c)}
                },

                new[]
                {
                    typeof(User)
                }
            };

            yield return new object[]
            {
                new Dictionary<Type, object>
                {
                    {typeof(User), new Validator<User>(c => c)},
                    {typeof(Company), new Validator<Company>(c => c)}
                },

                new[]
                {
                    typeof(User),
                    typeof(Company)
                }
            };
        }

        [Theory]
        [MemberData(nameof(RegisteredTypes_Data))]
        public void RegisteredTypes_Should_GatherRegisteredTypes(IReadOnlyDictionary<Type, object> validators, IReadOnlyCollection<Type> expectedRegisteredTypes)
        {
            var validatorsRepository = new ValidatorsRepository(validators);

            Assert.Equal(expectedRegisteredTypes.Count(), validatorsRepository.Types.Count());

            foreach (var expected in expectedRegisteredTypes)
            {
                Assert.Contains(expected, validatorsRepository.Types);
            }
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_NullDictionary()
        {
            Assert.Throws<ArgumentNullException>(() => new ValidatorsRepository(null));
        }

        [Fact]
        public void Get_Should_ReturnValidator()
        {
            var userValidator = new Validator<User>(c => c);
            var addressValidator = new Validator<Company>(c => c);

            var validatorsRepository = new ValidatorsRepository(new Dictionary<Type, object>
            {
                {typeof(User), userValidator},
                {typeof(Company), addressValidator}
            });

            Assert.Same(userValidator, validatorsRepository.Get<User>());
            Assert.Same(addressValidator, validatorsRepository.Get<Company>());
        }

        [Fact]
        public void Get_Should_ThrowException_ValidatorNotFound()
        {
            var validatorsRepository = new ValidatorsRepository(new Dictionary<Type, object>
            {
                {typeof(User), new Validator<User>(c => c)}
            });

            validatorsRepository.Get<User>();

            Assert.Throws<ValidatorNotFoundException>(() => { validatorsRepository.Get<Company>(); });
        }

        [Fact]
        public void Get_Should_ThrowException_When_InvalidValidatorRegistered()
        {
            var validatorsRepository = new ValidatorsRepository(new Dictionary<Type, object>
            {
                {typeof(User), new Validator<User>(c => c)},
                {typeof(Company), new Validator<User>(c => c)}
            });

            validatorsRepository.Get<User>();

            Assert.Throws<InvalidValidatorTypeException>(() => { validatorsRepository.Get<Company>(); });
        }
    }
}