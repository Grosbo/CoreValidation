using System;
using System.Collections.Generic;
using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    public interface IMemberSpecification<out TModel, out TMember>
        where TModel : class
    {
        IMemberSpecification<TModel, TMember> ValidRelative(Predicate<TModel> isValid, string message, IReadOnlyCollection<IMessageArg> args = null);

        IMemberSpecification<TModel, TMember> Valid(Predicate<TMember> isValid, string message, IReadOnlyCollection<IMessageArg> args = null);

        IMemberSpecification<TModel, TMember> WithSummaryError(string message, IReadOnlyCollection<IMessageArg> args = null);

        IMemberSpecification<TModel, TMember> WithName(string name);
    }
}