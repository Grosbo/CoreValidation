using System;
using System.Collections.Generic;
using CoreValidation.Errors.Args;

namespace CoreValidation.Specifications
{
    public interface IMemberSpecificationBuilder<out TModel, out TMember>
        where TModel : class
    {
        IMemberSpecificationBuilder<TModel, TMember> AsRelative(Predicate<TModel> isValid);

        IMemberSpecificationBuilder<TModel, TMember> Valid(Predicate<TMember> isValid);

        IMemberSpecificationBuilder<TModel, TMember> Valid(Predicate<TMember> isValid, string message, IReadOnlyCollection<IMessageArg> args = null);

        IMemberSpecificationBuilder<TModel, TMember> SetSingleError(string message);

        IMemberSpecificationBuilder<TModel, TMember> WithMessage(string message);
    }
}