using System;
using System.Linq;
using CoreValidation.Options;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation.Factory.Specifications
{
    public static class SpecificationsOptionsExtension
    {
        public static IValidationContextOptions AddSpecification<T>(this IValidationContextOptions options, Specification<T> specification)
            where T : class
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            return OptionsUnwrapper.UnwrapSpecifications(options, specifications =>
            {
                if (specifications.ContainsKey(typeof(T)))
                {
                    specifications[typeof(T)] = specification;
                }
                else
                {
                    specifications.Add(typeof(T), specification);
                }
            });
        }

        public static IValidationContextOptions AddSpecificationsFromHolder(this IValidationContextOptions options, ISpecificationHolder specificationsHolder)
        {
            if (specificationsHolder == null)
            {
                throw new ArgumentNullException(nameof(specificationsHolder));
            }

            var specifiedTypes = SpecificationHoldersHelpers.GetSpecifiedTypes(specificationsHolder.GetType());

            if (!specifiedTypes.Any())
            {
                throw new InvalidOperationException($"Type passed as {nameof(specificationsHolder)} should implement at least one {typeof(ISpecificationHolder<>).Name} type");
            }

            foreach (var specifiedType in specifiedTypes)
            {
                SpecificationHoldersHelpers.InvokeAddSpecificationFromHolder(options, specificationsHolder, specifiedType);
            }

            return options;
        }
    }
}