using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreValidation.Options;

namespace CoreValidation.Factory.Validators
{
    internal static class ValidatorHoldersHelpers
    {
        public static void InvokeAddValidator(IValidationContextOptions options, object holderInstance, Type validatedType)
        {
            var methods = typeof(ValidatorHoldersHelpers).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            var method = methods.First(m => m.Name == nameof(AddValidatorFromHolder)).MakeGenericMethod(validatedType);

            method.Invoke(null, new[] {options, holderInstance});
        }

        private static void AddValidatorFromHolder<T>(IValidationContextOptions options, IValidatorHolder<T> validatorHolder)
            where T : class
        {
            if (validatorHolder?.Validator == null)
            {
                throw new InvalidOperationException("Invalid (null?) validator from holders");
            }

            options.AddValidator(validatorHolder.Validator);
        }

        public static IReadOnlyCollection<Type> GetValidatedTypes(Type type)
        {
            return type.GetInterfaces()
                .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IValidatorHolder<>)))
                .Select(i => i.GetGenericArguments()[0])
                .ToArray();
        }
    }
}