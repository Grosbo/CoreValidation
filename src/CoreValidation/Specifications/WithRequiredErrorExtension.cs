using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class WithRequiredErrorExtension
    {
        public static IMemberSpecification<TModel, TMember> WithRequiredError<TModel, TMember>(this IMemberSpecification<TModel, TMember> builder, string errorMessage, IReadOnlyCollection<IMessageArg> args = null)
            where TModel : class
            where TMember : class
        {
            return ((MemberSpecification<TModel, TMember>) builder).WithRequiredError(errorMessage, args);
        }

        public static IMemberSpecification<TModel, TMember?> WithRequiredError<TModel, TMember>(this IMemberSpecification<TModel, TMember?> builder, string errorMessage, IReadOnlyCollection<IMessageArg> args = null)
            where TModel : class
            where TMember : struct
        {
            return ((MemberSpecification<TModel, TMember?>) builder).WithRequiredError(errorMessage, args);
        }
    }
}