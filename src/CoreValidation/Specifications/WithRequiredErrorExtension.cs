using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class WithRequiredErrorExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember> WithRequiredError<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember> builder, string errorMessage, IReadOnlyCollection<IMessageArg> args = null)
            where TModel : class
            where TMember : class
        {
            return ((MemberSpecificationBuilder<TModel, TMember>) builder).WithRequiredError(errorMessage, args);
        }

        public static IMemberSpecificationBuilder<TModel, TMember?> WithRequiredError<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember?> builder, string errorMessage, IReadOnlyCollection<IMessageArg> args = null)
            where TModel : class
            where TMember : struct
        {
            return ((MemberSpecificationBuilder<TModel, TMember?>) builder).WithRequiredError(errorMessage, args);
        }
    }
}