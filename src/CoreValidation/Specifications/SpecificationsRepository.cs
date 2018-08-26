using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CoreValidation.Exceptions;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal sealed class SpecificationsRepository : ISpecificationsRepository
    {
        private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();
        private readonly IValidatorsRepository _validatorsRepository;

        public SpecificationsRepository(IValidatorsRepository validatorsRepository)
        {
            _validatorsRepository = validatorsRepository ?? throw new ArgumentNullException(nameof(validatorsRepository));
        }

        public IEnumerable<string> Keys
        {
            get => _cache.Keys;
        }

        public ISpecification<T> Get<T>(string key = null) where T : class
        {
            var cacheKey = ResolveCacheKey<T>(key);

            return _cache[cacheKey] as ISpecification<T>;
        }

        public ISpecification<T> GetOrInit<T>(Validator<T> validator = null, string key = null) where T : class
        {
            var cacheKey = ResolveCacheKey<T>(key);

            return _cache.GetOrAdd(cacheKey, _ =>
            {
                var initSpecification = new SpecificationBuilder<T>();

                var finalValidator = validator ?? _validatorsRepository.Get<T>();

                var processedSpecification = finalValidator(initSpecification);

                if (!ReferenceEquals(initSpecification, processedSpecification) || !(processedSpecification is SpecificationBuilder<T>))
                {
                    throw new InvalidProcessedReferenceException(typeof(SpecificationBuilder<T>));
                }

                return processedSpecification;
            }) as ISpecification<T>;
        }

        private string ResolveCacheKey<T>(string key)
        {
            return key ?? typeof(T).FullName ?? throw new InvalidOperationException("Cannot resolve cache key");
        }
    }
}