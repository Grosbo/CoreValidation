using System;
using System.Linq;
using CoreValidation.Factory.Specifications;
using CoreValidation.Options;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class SpecificationsOptionsExtension
    {
        /// <summary>
        ///     Adds specification of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="specification">Specification of type <typeparamref name="T" /></param>
        /// <typeparam name="T">Specified type.</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="specification" /> is null.</exception>
        public static IValidationContextOptions AddSpecification<T>(this IValidationContextOptions options, Specification<T> specification)
            where T : class
        {
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

        /// <summary>
        ///     Add all specifications from the selected <see cref="ISpecificationHolder" />.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="specificationsHolder">
        ///     Object that contains specifications. Should implement at least one
        ///     <see cref="ISpecificationHolder{T}" />.
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="specificationsHolder" /> is null.</exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if <paramref name="specificationsHolder" /> doesn't contain
        ///     specifications.
        /// </exception>
        public static IValidationContextOptions AddSpecificationsFromHolder(this IValidationContextOptions options, ISpecificationHolder specificationsHolder)
        {
            if (specificationsHolder == null)
            {
                throw new ArgumentNullException(nameof(specificationsHolder));
            }

            var specifiedTypes = SpecificationHoldersHelpers.GetSpecifiedTypes(specificationsHolder.GetType());

            if (!specifiedTypes.Any())
            {
                throw new InvalidSpecificationHolderException($"Type passed as {nameof(specificationsHolder)} should implement at least one {typeof(ISpecificationHolder<>).Name} type");
            }

            foreach (var specifiedType in specifiedTypes)
            {
                SpecificationHoldersHelpers.InvokeAddSpecificationFromHolder(options, specificationsHolder, specifiedType);
            }

            return options;
        }
    }
}