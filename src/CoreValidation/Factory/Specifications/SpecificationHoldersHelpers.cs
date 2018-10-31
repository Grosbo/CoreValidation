using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreValidation.Exceptions;
using CoreValidation.Options;

namespace CoreValidation.Factory.Specifications
{
    internal static class SpecificationHoldersHelpers
    {
        public static void InvokeAddSpecificationFromHolder(IValidationContextOptions options, object holderInstance, Type specifiedType)
        {
            var methods = typeof(SpecificationHoldersHelpers).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            var method = methods.First(m => m.Name == nameof(AddSpecificationFromHolder)).MakeGenericMethod(specifiedType);

            try
            {
                method.Invoke(null, new[] {options, holderInstance});
            }
            catch (Exception ex)
            {
                if (ex.InnerException is InvalidSpecificationHolderException)
                {
                    throw ex.InnerException;
                }

                throw;
            }
        }

        private static void AddSpecificationFromHolder<T>(IValidationContextOptions options, ISpecificationHolder<T> specificationHolder)
            where T : class
        {
            if (specificationHolder?.Specification == null)
            {
                throw new InvalidSpecificationHolderException("Invalid (null?) specification from holders");
            }

            options.AddSpecification(specificationHolder.Specification);
        }

        public static IReadOnlyCollection<Type> GetSpecifiedTypes(Type type)
        {
            return type.GetInterfaces()
                .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ISpecificationHolder<>)))
                .Select(i => i.GetGenericArguments()[0])
                .ToArray();
        }
    }
}