using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;

namespace CoreValidation.Validators
{
    internal sealed class ValidatorsFactory : IValidatorsFactory
    {
        private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();
        private readonly ISpecificationsRepository _specificationsRepository;

        public ValidatorsFactory(ISpecificationsRepository specificationsRepository)
        {
            _specificationsRepository = specificationsRepository ?? throw new ArgumentNullException(nameof(specificationsRepository));
        }

        public IEnumerable<string> Keys => _cache.Keys;

        public IValidator<T> Get<T>(string key = null) where T : class
        {
            var cacheKey = ResolveCacheKey<T>(key);

            return _cache[cacheKey] as IValidator<T>;
        }

        public IValidator<T> GetOrInit<T>(Specification<T> specification = null, string key = null) where T : class
        {
            var cacheKey = ResolveCacheKey<T>(key);

            return _cache.GetOrAdd(cacheKey, _ =>
            {
                var initSpecification = new SpecificationBuilder<T>();

                var finalSpecification = specification ?? _specificationsRepository.Get<T>();

                var processedSpecification = finalSpecification(initSpecification);

                if (!ReferenceEquals(initSpecification, processedSpecification) || !(processedSpecification is SpecificationBuilder<T>))
                {
                    throw new InvalidProcessedReferenceException(typeof(SpecificationBuilder<T>));
                }

                return processedSpecification;
            }) as IValidator<T>;
        }

        private string ResolveCacheKey<T>(string key)
        {
            return key ?? typeof(T).FullName ?? throw new InvalidOperationException("Cannot resolve cache key");
        }
    }
}