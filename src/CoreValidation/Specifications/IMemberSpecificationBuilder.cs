using System;
using System.Collections.Generic;
using CoreValidation.Errors.Args;

namespace CoreValidation.Specifications
{
    public interface IMemberSpecificationBuilder<out TModel, out TMember>
        where TModel : class
    {
        IMemberSpecificationBuilder<TModel, TMember> ValidRelative(Predicate<TModel> isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null);

        IMemberSpecificationBuilder<TModel, TMember> Valid(Predicate<TMember> isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null);

        IMemberSpecificationBuilder<TModel, TMember> WithSummaryError(string message, IReadOnlyCollection<IMessageArg> args = null);

        IMemberSpecificationBuilder<TModel, TMember> WithName(string name);
    }
}