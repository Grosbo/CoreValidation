using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CoreValidation.Validators
{
    public sealed class ValidatorsRepository : IValidatorsRepository
    {
        private readonly ConcurrentDictionary<Type, object> _validators;

        public ValidatorsRepository(IReadOnlyDictionary<Type, object> validators)
        {
            if (validators == null)
            {
                throw new ArgumentNullException(nameof(validators));
            }

            _validators = new ConcurrentDictionary<Type, object>(validators);
            Types = _validators.Keys.ToArray();
        }

        public IReadOnlyCollection<Type> Types { get; }

        public Validator<T> Get<T>()
            where T: class
        {
            if (!_validators.TryGetValue(typeof(T), out var validationFunction))
            {
                throw new ValidatorNotFoundException(typeof(T));
            }

            return validationFunction as Validator<T>
                ?? throw new InvalidValidatorTypeException(typeof(T), validationFunction);
        }
    }
}