using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CoreValidation.Specifications
{
    public sealed class SpecificationsRepository : ISpecificationsRepository
    {
        private readonly ConcurrentDictionary<Type, object> _specifications;

        public SpecificationsRepository(IReadOnlyDictionary<Type, object> validators)
        {
            if (validators == null)
            {
                throw new ArgumentNullException(nameof(validators));
            }

            _specifications = new ConcurrentDictionary<Type, object>(validators);
            Types = _specifications.Keys.ToArray();
        }

        public IReadOnlyCollection<Type> Types { get; }

        public Specification<T> Get<T>()
            where T : class
        {
            if (!_specifications.TryGetValue(typeof(T), out var validationFunction))
            {
                throw new SpecificationNotFoundException(typeof(T));
            }

            return validationFunction as Specification<T>
                   ?? throw new InvalidSpecificationTypeException(typeof(T), validationFunction);
        }
    }
}