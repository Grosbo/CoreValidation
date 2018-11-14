using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class SetRequiredErrorExtension
    {
        /// <summary>
        /// Sets the error message for the member if its value is null. Overrides the default from the validation context settings.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TMember">Type of the member's nullable underlying model.</typeparam>
        /// <param name="builder"></param>
        /// <param name="errorMessage">The error message.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessage" /> is null</exception>
        public static IMemberSpecificationBuilder<TModel, TMember> SetRequired<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember> builder, string errorMessage)
            where TModel : class
            where TMember : class
        {
            return ((MemberSpecificationBuilder<TModel, TMember>)builder).SetRequired(errorMessage);
        }

        /// <summary>
        /// Sets the error message for the member if its value is null. Overrides the default from the validation context settings.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TMember">Type of the member's nullable underlying model.</typeparam>
        /// <param name="builder"></param>
        /// <param name="errorMessage">The error message.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessage" /> is null</exception>
        public static IMemberSpecificationBuilder<TModel, TMember?> SetRequired<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember?> builder, string errorMessage)
            where TModel : class
            where TMember : struct
        {
            return ((MemberSpecificationBuilder<TModel, TMember?>)builder).SetRequired(errorMessage);
        }
    }
}