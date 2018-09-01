using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class SpecificationsRepositoryTests
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
                    {typeof(User), new Specification<User>(c => c)}
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
                    {typeof(User), new Specification<User>(c => c)},
                    {typeof(Company), new Specification<Company>(c => c)}
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
        public void RegisteredTypes_Should_GatherRegisteredTypes(IReadOnlyDictionary<Type, object> specifications, IReadOnlyCollection<Type> expectedRegisteredTypes)
        {
            var specificationsRepository = new SpecificationsRepository(specifications);

            Assert.Equal(expectedRegisteredTypes.Count(), specificationsRepository.Types.Count());

            foreach (var expected in expectedRegisteredTypes)
            {
                Assert.Contains(expected, specificationsRepository.Types);
            }
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_NullDictionary()
        {
            Assert.Throws<ArgumentNullException>(() => new SpecificationsRepository(null));
        }

        [Fact]
        public void Get_Should_ReturnSpecification()
        {
            var userSpecification = new Specification<User>(c => c);
            var addressSpecification = new Specification<Company>(c => c);

            var specificationsRepository = new SpecificationsRepository(new Dictionary<Type, object>
            {
                {typeof(User), userSpecification},
                {typeof(Company), addressSpecification}
            });

            Assert.Same(userSpecification, specificationsRepository.Get<User>());
            Assert.Same(addressSpecification, specificationsRepository.Get<Company>());
        }

        [Fact]
        public void Get_Should_ThrowException_SpecificationNotFound()
        {
            var specificationsRepository = new SpecificationsRepository(new Dictionary<Type, object>
            {
                {typeof(User), new Specification<User>(c => c)}
            });

            specificationsRepository.Get<User>();

            Assert.Throws<SpecificationNotFoundException>(() => { specificationsRepository.Get<Company>(); });
        }

        [Fact]
        public void Get_Should_ThrowException_When_InvalidSpecificationRegistered()
        {
            var specificationsRepository = new SpecificationsRepository(new Dictionary<Type, object>
            {
                {typeof(User), new Specification<User>(c => c)},
                {typeof(Company), new Specification<User>(c => c)}
            });

            specificationsRepository.Get<User>();

            Assert.Throws<InvalidSpecificationTypeException>(() => { specificationsRepository.Get<Company>(); });
        }
    }
}